using HtmlAgilityPack;
using System.Net.Http.Headers;

public class DgiiScraperService(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<Dictionary<string, string>> ConsultarRnc(string rnc)
    {
        try
        {
            var url = "https://www.dgii.gov.do/app/WebApps/ConsultasWeb/consultas/rnc.aspx";

            var initialResponse = await _httpClient.GetAsync(url);
            var initialHtml = await initialResponse.Content.ReadAsStringAsync();

            var doc = new HtmlDocument();
            doc.LoadHtml(initialHtml);

            string viewState = doc.DocumentNode.SelectSingleNode("//input[@id='__VIEWSTATE']")?.GetAttributeValue("value", "") ?? "";
            string eventValidation = doc.DocumentNode.SelectSingleNode("//input[@id='__EVENTVALIDATION']")?.GetAttributeValue("value", "") ?? "";

            var formData = new Dictionary<string, string>
        {
            { "__VIEWSTATE", viewState },
            { "__EVENTVALIDATION", eventValidation },
            { "__EVENTTARGET", "" },
            { "__EVENTARGUMENT", "" },
            { "ctl00$cphMain$txtRNCCedula", rnc },
            { "ctl00$cphMain$btnBuscarPorRNC", "Buscar" }
        };

            var content = new FormUrlEncodedContent(formData);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var postResponse = await _httpClient.PostAsync(url, content);
            var responseHtml = await postResponse.Content.ReadAsStringAsync();

            var resultDoc = new HtmlDocument();
            resultDoc.LoadHtml(responseHtml);

            var table = resultDoc.DocumentNode.SelectSingleNode("//table[@id='ctl00_cphMain_dvDatosContribuyentes']");
            if (table == null)
                return new Dictionary<string, string> { { "error", "No se encontraron resultados para ese RNC o Cédula." } };

            var rows = table.SelectNodes(".//tr");
            if (rows == null)
                return new Dictionary<string, string> { { "error", "No se encontraron filas en la tabla de resultados." } };

            var result = new Dictionary<string, string>();
            foreach (var row in rows)
            {
                var cells = row.SelectNodes(".//td");
                if (cells?.Count != 2) continue;

                string key = HtmlEntity.DeEntitize(cells[0].InnerText.Trim());
                string value = HtmlEntity.DeEntitize(cells[1].InnerText.Trim());
                result[key] = value;
            }

            return result;
        }
        catch (Exception ex)
        {
            return new Dictionary<string, string> { { "error", "Ocurrió un error procesando la solicitud: " + ex.Message } };
        }
    }

}
