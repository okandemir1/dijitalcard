namespace DijitalCard.WebUI.Management.Helpers
{
    using System.Linq;
    using System.Text;
    using DijitalCard.WebUI.Management.Models;

    public class ViewResultExtension
    {
        public static string ViewResult(ViewModelResult viewModelResult)
        {
            if (viewModelResult == null)
                return "";

            if(viewModelResult.Errors.Count() > 0 && !viewModelResult.IsSucceed)
            {
                var return_html = new StringBuilder($"<div class='card p-4 mb-3 bg-danger text-white shadow'><p>{viewModelResult.Message}</p><ul>");
                foreach (var item in viewModelResult.Errors)
                {
                    return_html.Append($"<li>{item}</li>");
                }
                return_html.Append("</ul></div>");

                return return_html.ToString();
            }

            if(!viewModelResult.IsSucceed && !string.IsNullOrEmpty(viewModelResult.Message))
            {
                var return_html = new StringBuilder($"<div class='card p-4 mb-3 bg-danger text-white shadow'>Hata : {viewModelResult.Message}</div>");
                return return_html.ToString();
            }

            if(viewModelResult.IsSucceed && !string.IsNullOrEmpty(viewModelResult.Message))
            {
                var return_html = new StringBuilder($"<div class='card p-4 mb-3 bg-success text-white shadow'>Başarılı : {viewModelResult.Message}</div>");
                return return_html.ToString();
            }

            return "";
        }
    }
}
