using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GridViewBatchEdit.Helpers;

namespace GridViewBatchEdit.Models {
    public interface IDataStorage {
        List<GridDataItem> GetData();
        void InsertDataItem(GridDataItem item);
        void UpdateDataItem(GridDataItem item);
        void DeleteDataItem(int id);
    }
    public class MemoryDataStorage : IDataStorage {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string sessionKey = "_gridDataStorage";
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        public MemoryDataStorage(IHttpContextAccessor httpContextAccessor) {
            _httpContextAccessor = httpContextAccessor;
            SaveData(Enumerable.Range(0, 10).Select(i => new GridDataItem(i, "Name" + i, (decimal)i * 1.1m)).ToList());
        }
        public List<GridDataItem> GetData() {
            return _session.GetObjectFromJson<List<GridDataItem>>(sessionKey) ?? new List<GridDataItem>();
        }
        public void InsertDataItem(GridDataItem item) {
            List<GridDataItem> data = GetData();
            item.Id = data.Max(i => i.Id) + 1;
            data.Add(item);
            SaveData(data);
        }
        public void UpdateDataItem(GridDataItem item) {
            List<GridDataItem> data = GetData();
            GridDataItem itemToUpdate = data.Find(i => i.Id == item.Id);
            itemToUpdate.Name = item.Name;
            itemToUpdate.Price = item.Price;
            SaveData(data);
        }
        public void DeleteDataItem(int id) {
            List<GridDataItem> data = GetData();
            data.Remove(data.Find(i => i.Id == id));
            SaveData(data);
        }
        private void SaveData(List<GridDataItem> items) {
            _session.SetObjectAsJson(sessionKey, items);
        }
    }
}
