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

const tabKey = 9;
const enterKey = 13;
const escKey = 27;
var preventEndEditOnLostFocus = false;
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