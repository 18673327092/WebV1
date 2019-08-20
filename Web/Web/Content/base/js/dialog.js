var dlg = {
    openContent: function (_options, fn1, fn2) {
        var options = {
            type: 1,
            title: "信息",
            content: "",
            offset: ['86px'],
            skin: 'layui-layer-demo', //样式类名
            closeBtn: 1, //不显示关闭按钮
            anim: 5,
            area: ['620px'], //宽高
            shadeClose: false, //开启遮罩关闭
            btn: ['确定', '取消'],
            yes: function (index) {
                if (typeof fn1 == "function") {
                    fn1();
                }
            },
            cancel: function (index) {
                layer.closeAll(index);
                if (typeof fn2 == "function") {
                    fn2();
                }
            }
        };
        $.extend(options, _options || {});
        layer.open(options);
    },
    openPageHtml: function (_options, fn1, fn2) {
        var options = {
            type: 1,
            title: "信息",
            offset: ['20px'],
            skin: 'layui-layer-demo', //样式类名
            closeBtn: 1, //不显示关闭按钮
            anim: 5,
            area: ['620px'], //宽高
            shadeClose: false, //开启遮罩关闭
            btn: ['确定', '取消'],
            yes: function (index) {
                if (typeof fn1 == "function") {
                    fn1();
                }
            },
            cancel: function (index) {
                layer.closeAll(index);
                if (typeof fn2 == "function") {
                    fn2();
                }
            }
        };
        $.extend(options, _options || {});
        layer.msg("页面加载中...", {}, function () { });
        $.get(options.url, {}, function (data) {
            layer.closeAll();
            options.content = data;
            layer.open(options);
        }, "html")
    },
    openframe: function (options, fn1, fn2) {
        var _options = {
            title: "",
            offset: ['10px'],
            type: 2,
            moveOut: false,
            //shade: 0,
            area: ['85%', '96%'],
            fixed: true, //不固定
            maxmin: true,
            shadeClose: false, //开启遮罩关闭
            yes: function (index) {
                layer.closeAll(index);
                if (typeof fn1 == "function") {
                    fn1();
                }
            },
            cancel: function (index) {
                layer.closeAll(index);
                if (typeof fn2 == "function") {
                    fn2();
                }
            },
            min: function (layero) { $(".layui-layer-shade").hide(); },
            full: function (layero) { $(".layui-layer-shade:eq(0)").show(); },
            restore: function (layero) { $(".layui-layer-shade:eq(0)").show(); }
        }

        $.extend(_options, options || {});
        var index = layer.open(_options);
        $(".layui-layer").mousemove(function () {
            $(".layui-layer").css({ "z-index": 119999999 });
            $(this).css({ "z-index": 120000000 });
        });

        return index
    },
    confirm: function (message, fnOk, fnCancel) {
        layer.confirm(message, {
            title: "系统提示",
            offset: ['86px'],
            zIndex: 1299999999,
            btn: ['确定', '取消'] //按钮
        }, function () {
            if (typeof fnOk == "function") {
                fnOk();
            }
            layer.closeAll();
        }, function () {
            if (typeof fnCancel == "function") {
                fnCancel();
            }
        });
    },
    alert: function (message, callback) {
        layer.alert(message, { title: "系统提示", offset: ['86px'], zIndex: 120000001 }, callback);
    },
    _msg: function (message, options, callback) {
        try {
            if (message == "" || message == "undefined") return false;
            var _options = { offset: "t", zIndex: 120000001, };//offset: 't', time: 1000
            $.extend(_options, options || {});
            layer.msg(message, _options, callback);
        } catch (e) {
            layer.closeAll();
        }
    },
    msg: {
        success: function (message, options, callback) {
            dlg._msg(message, $.extend(options, { icon: 6 } || {}), callback)
        },
        error: function (message, options, callback) {
            dlg._msg(message, $.extend(options, { icon: 5 } || {}), callback)
        },
        question: function (message, options, callback) {
            dlg._msg(message, $.extend(options, { icon: 3 } || {}), callback)
        },
        lock: function (message, options, callback) {
            dlg._msg(message, $.extend(options, { icon: 4 } || {}), callback)
        },
        info: function (message, options, callback) {
            dlg._msg(message, $.extend({ offset: "60px" }, options || {}), callback)
        },
    },
    load: function (t) {
        if (!t) t = 1;
        layer.load(t, { shade: false });
    },
    closeAll: function () {
        layer.closeAll();
    },
    close: function (index) {
        layer.closeAll(index);
    }
}

