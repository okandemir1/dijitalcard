namespace DijitalCard.WebUI.Management.Models
{
    using System.Collections.Generic;

    public class ViewModelResult
    {
        public ViewModelResult()
        {
            IsSucceed = true;
            Errors = new List<string>();
        }

        public ViewModelResult(bool _isSucceed, string _message)
        {
            IsSucceed = _isSucceed;
            Message = _message;
            Errors = new List<string>();
        }

        public ViewModelResult(bool _isSucceed, string _message, string _error)
        {
            IsSucceed = _isSucceed;
            Message = _message;
            Errors = string.IsNullOrEmpty(_error) ? new List<string>() : new List<string>() { _error };
        }

        public ViewModelResult(bool _isSucceed, string _message, List<string> _errors)
        {
            IsSucceed = _isSucceed;
            Message = _message;
            Errors = _errors;
        }

        public bool IsSucceed { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
    }
}
