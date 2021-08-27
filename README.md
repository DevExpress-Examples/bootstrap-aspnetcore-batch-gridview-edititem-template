<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/134857394/18.1.3%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T830585)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->
# BootstrapGridView for ASP.NET Core - Batch Edit - How to implement an edit item template
This example shows how to implement an edit item template that will work with the Batch Edit mode. The template contains the BootstrapSpinEdit control.

## Steps to implement:
* Place the editor inside the Edit Item template

```csharp
    columns.Add(m => m.Price).EditItemTemplate(t => @<text>
            @(Html.DevExpress().BootstrapSpinEdit("priceSpinEdit")...)
            </text>); ;
    })
```

* Handle the BatchEditStartEditing, BatchEditEndEditing and BatchEditRowValidating client-side events - they will interact with the template editor:

```javascript
function onBatchEditStartEditing(e) {
    var templateColumn = this.getColumnByField("Price");
    if (!e.rowValues.hasOwnProperty(templateColumn.index))
        return;
    var cellInfo = e.rowValues[templateColumn.index];
    priceSpinEdit.setValue(cellInfo.value);
    if (e.focusedColumn === templateColumn)
        priceSpinEdit.focus();
}
function onBatchEditEndEditing(e) {
    var templateColumn = this.getColumnByField("Price");
    if (!e.rowValues.hasOwnProperty(templateColumn.index))
        return;
    var cellInfo = e.rowValues[templateColumn.index];
    cellInfo.value = priceSpinEdit.getValue();
    cellInfo.text = priceSpinEdit.getText();
    priceSpinEdit.setValue(null);
}
function onBatchEditRowValidating(e) {
    var templateColumn = this.getColumnByField("Price");
    var cellValidationInfo = e.validationInfo[templateColumn.index];
    if (!cellValidationInfo) return;
    var value = cellValidationInfo.value;
    if (!value) {
        cellValidationInfo.isValid = false;
        cellValidationInfo.errorText = "Price is required";
    }
}
```
* Handle the Spin Edit client-side events:

```csharp 
@(Html.DevExpress().BootstrapSpinEdit("priceSpinEdit")
            .ClientSideEvents(clientSideEvents => clientSideEvents.KeyDown("onKeyDown").LostFocus("onLostFocus")))
```

The LostFocus handler will stop editing when the focus is moved to another element, and the KeyDown handler will allow navigation/cancelling editing with the Tab/Esc keys:
```javascript
function onKeyDown(e) {
    var keyCode = e.htmlEvent.keyCode;
    if (keyCode === escKey) {
        var cellInfo = gridView.batchEditApi.getEditCellInfo();
        window.setTimeout(function () {
            gridView.setFocusedCell(cellInfo.rowVisibleIndex, cellInfo.column.index);
        }, 0);
        this.getInputElement().blur();
        return;
    }
    if (keyCode !== tabKey && keyCode !== enterKey) return;
    var moveActionName = e.htmlEvent.shiftKey ? "moveFocusBackward" : "moveFocusForward";
    if (gridView.batchEditApi[moveActionName]()) {
        e.htmlEvent.preventDefault();
        preventEndEditOnLostFocus = true;
    }
}
function onLostFocus(e) {
    if (!preventEndEditOnLostFocus)
        gridView.batchEditApi.endEdit();
    preventEndEditOnLostFocus = false;
}
```
