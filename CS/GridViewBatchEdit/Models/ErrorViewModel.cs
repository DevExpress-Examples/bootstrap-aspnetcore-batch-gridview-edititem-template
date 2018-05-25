using System;

namespace GridViewBatchEdit.Models {
    public class ErrorViewModel {
        public string RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}