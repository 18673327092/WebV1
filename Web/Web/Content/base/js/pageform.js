var pageform = {
    //只读文本框赋值
    setReadonlyInput: function (field, value) {
        $("#span_" + field).html(value);
        $("#" + field).val(value);
    },
    //只读选择文本框赋值
    setReadonlySelectInput: function (field, value, title) {
        $("#span_" + field).html(title);
        $("#" + field).val(value);
    },
    //只读选择框赋值
    setReadonlySpan: function (fieldid, value, title) {
        debugger
        title = decodeURIComponent(title);
        $("#span_" + fieldid + "0").html(title).css("background-color", "#FBFBFB");
        $("#input_" + fieldid + "0").val(value);
        $("#icon_" + fieldid + "0").attr("data-value", value).hide();
    },
    //选择框赋值
    setSpan: function (fieldid, value, title) {
        $("#span_" + fieldid + "0").html(title);
        $("#input_" + fieldid + "0").val(value);
        $("#icon_" + fieldid + "0").attr("data-value", value);
    },

};