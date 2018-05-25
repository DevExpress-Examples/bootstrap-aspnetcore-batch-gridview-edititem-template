using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GridViewBatchEdit.Models;

namespace GridViewBatchEdit.Controllers {
    public class HomeController : Controller {
        public HomeController(IDataStorage storage) {
            Storage = storage ?? throw new ArgumentNullException(nameof(storage));
        }
        public IDataStorage Storage { get; }

        public IActionResult Index() {
            return View(Storage.GetData());
        }

        public IActionResult GridViewPartial() {
            return PartialView(Storage.GetData());
        }

        public IActionResult GridViewBatchUpdate(DevExpress.AspNetCore.Bootstrap.BootstrapGridBatchUpdateValues<GridDataItem, int> values) {
            foreach (GridDataItem item in values.Insert)
                if (values.IsValid(item))
                    try {
                        Storage.InsertDataItem(item);
                    } catch (Exception e) {
                        values.SetErrorText(item, e.Message);
                    }
            foreach (GridDataItem item in values.Update)
                if (values.IsValid(item))
                    try {
                        Storage.UpdateDataItem(item);
                    } catch (Exception e) {
                        values.SetErrorText(item, e.Message);
                    }
            foreach (int key in values.DeletedKeys)
                try {
                    Storage.DeleteDataItem(key);
                } catch (Exception e) {
                    values.SetErrorText(key, e.Message);
                }

            return PartialView("GridViewPartial", Storage.GetData());
        }

        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}