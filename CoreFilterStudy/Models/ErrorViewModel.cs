using System;

namespace CoreFilterStudy.Models
{
    public class ErrorViewModel
    {
        public ErrorViewModel()
        {
            Console.WriteLine("ErrorViewModel������");
        }
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
