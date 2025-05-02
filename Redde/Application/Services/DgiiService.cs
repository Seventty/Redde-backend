using System.Net.Http.Headers;
using HtmlAgilityPack;

public class DgiiService
{
    private readonly HttpClient _http;

    public DgiiService(HttpClient http)
    {
        _http = http;
    }

    public async Task<DgiiResponse> ConsultarRNC(string rnc)
    {
        var baseUrl = "https://dgii.gov.do/app/WebApps/ConsultasWeb2/ConsultasWeb/consultas/rnc.aspx";

        var initialResponse = await _http.GetAsync(baseUrl);
        var html = await initialResponse.Content.ReadAsStringAsync();

        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        string viewState = doc.DocumentNode
            .SelectSingleNode("//input[@id='__VIEWSTATE']")
            ?.GetAttributeValue("value", "") ?? "";

        string eventValidation = doc.DocumentNode
            .SelectSingleNode("//input[@id='__EVENTVALIDATION']")
            ?.GetAttributeValue("value", "") ?? "";

        var form = new Dictionary<string, string>
        {
            ["__VIEWSTATE"] = viewState,
            ["__EVENTVALIDATION"] = eventValidation,
            ["ctl00$ctl00$ctl00$MainContent$MainContent$txtRncCedula"] = rnc,
            ["ctl00$ctl00$ctl00$MainContent$MainContent$btnBuscarPorRNC"] = "Buscar"
        };

        var postContent = new FormUrlEncodedContent(form);

        var postResponse = await _http.PostAsync(baseUrl, postContent);
        var resultHtml = await postResponse.Content.ReadAsStringAsync();

        var resultDoc = new HtmlDocument();
        resultDoc.LoadHtml(resultHtml);

        string nombre = resultDoc.DocumentNode
            .SelectSingleNode("//span[@id='MainContent_MainContent_lblNombre']")
            ?.InnerText.Trim() ?? "";

        string estado = resultDoc.DocumentNode
            .SelectSingleNode("//span[@id='MainContent_MainContent_lblEstado']")
            ?.InnerText.Trim() ?? "";

        string tipo = resultDoc.DocumentNode
            .SelectSingleNode("//span[@id='MainContent_MainContent_lblTipo']")
            ?.InnerText.Trim() ?? "";

        return new DgiiResponse
        {
            Rnc = rnc,
            Nombre = nombre,
            Estado = estado,
            Tipo = tipo
        };
    }
}

public class DgiiResponse
{
    public string Rnc { get; set; } = "";
    public string Nombre { get; set; } = "";
    public string Estado { get; set; } = "";
    public string Tipo { get; set; } = "";
}
