var formvalid = {
    process: 0,
    form: $("#form-submit"),
    layer: { index: 0, loadindex: 0 },
    fn_beforeSendArr: [],
    fn_afterSubmitArr: [],
    fn_beforeSend: function () { },
    options: {},
    _ajaxformparams: function () {
        var _this = this;
        return {
            iframe: true,
            beforeSend: function () {
                var result = _this.fn_beforeSend();
                $.each(_this.fn_beforeSendArr, function (i, fun) {
                    if (typeof fun == "function") {
                        if (fun() == false) { result = false; }
                    }
                });
                if (result == false) {
                    _this.process = 0; return false;
                }
                _this.form.validate();
                _this.layer.loadindex = layer.load(2);
                dlg.msg.info("正在提交数据…");
            },
            success: function (data) {
                //表单提交完成以后触发的方法
                $.each(_this.fn_afterSubmitArr, function (i, fun) {
                    if (typeof fun == "function") {
                        fun(data);
                    }
                });
                if (data.Success) {
                    setTimeout(function () { layer.close(_this.layer.loadindex); }, 2000);
                    _this.formdata = "";
                } else {
                    layer.close(_this.layer.loadindex);
                    //自定义错误
                    if (data.Code == 1) {
                        dlg.msg.info(data.Message);
                    } else {
                        dlg.msg.info("保存失败，" + data.Message);
                    }
                }
                _this.process = 0;
            },
            error: function (arg1, arg2, ex) {
                //信息框-错误提示
                parent.dlg.msg.info(ex);
            },
            dataType: 'json'
        };
    },
    _submit: function () {
        var _this = this;
        var valid = _this.form.validate({
            onkeyup: true,
            onclick: true,
            errorPlacement: function (error, element) {
                // console.log(JSON.stringify(error));
            },
            showErrors: function (errorMap, errorList) {
                var message = "";
                var element = "";
                for (var m in errorMap) {
                    if (message == "") {
                        message = errorMap[m];
                        element = m;
                    }
                }
                $("#" + element).focus();
                dlg.msg.info(message);
                this.defaultShowErrors();
            }
        });
        if (valid.form()) {
            _this.process++;
            if (_this.process > 1) {
                parent.dlg.msg.info("请不要重复提交");
            } else {
                _this.form.submit();
            }
        }
    },
    init: function (options) {
        var _this = this;
        if (parent.layer && parent.layer.getFrameIndex) {
            _this.layer.index = parent.layer.getFrameIndex(window.name); //获取窗口索引
        }
        var option = _this._ajaxformparams();
        $.extend(option, options || {});
        _this.form.ajaxForm(option);
        _this.bind();
    },
    bind: function () {
        var _this = this;
        $("#btn_submit").on("click", function () { _this._submit(); });
        $("#btn_cancel").on("click", function () { parent.dlg.close(); });
    }
}