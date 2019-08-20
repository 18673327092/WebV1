﻿$(function () {
    $('.loginbox').css({ 'position': 'absolute', 'left': ($(window).width() - 692) / 2 });
    $(window).resize(function () {
        $('.loginbox').css({ 'position': 'absolute', 'left': ($(window).width() - 692) / 2 });
    });

    if (request("code") == "-1") {
        dlg.alert("由于你长时间未操作系统，已自动退出", function () {
            dlg.closeAll();
            location.href = "/Login";
        })
    }
    setTimeout(function () {
        $("#LoginPassword").attr("type", "password");
        if ($.cookie('LoginAccount')) {
            $("#LoginPassword").attr("type", "password")
            $("#isremeber").attr("checked", true);
            $("#LoginAccount").val(getCookie('LoginAccount'))
            $("#LoginPassword").val(getCookie('LoginPassword'))
        }
    }, 1000);

    var returnUrl = request("returnUrl");
    $("#login").Validform({
        ajaxPost: true,
        postonce: true,
        tipSweep: true,
        tiptype: function (msg, o, cssctl) {
            if (msg == "正在提交数据…") {
                $("#btn_submit").val("登录中…");
            } else {
                if (msg.indexOf("status: 500") != -1) {
                    dlg.msg.info("服务器网络异常");
                } else {
                    dlg.msg.info(msg);

                }
                $("#btn_submit").val("登录");
                return;
            }
        },
        showAllError: false,
        ajaxPost: true,
        beforeSubmit: function (curform) {
            //在验证成功后，表单提交前执行的函数，curform参数是当前表单对象。
            //这里明确return false的话表单将不会提交;
        },
        callback: function (data) {
            if (data.Success) {
                $("#btn_submit").val("登录成功");
                delCookie("LoginAccount");
                delCookie("LoginPassword");
                if ($("#isremeber").is(":checked")) {
                    var user = {
                        LoginAccount: $("#LoginAccount").val(),
                        LoginPassword: $("#LoginPassword").val()
                    }
                    setCookie("LoginAccount", user.LoginAccount);
                    setCookie("LoginPassword", user.LoginPassword);
                }
                setTimeout(function () {
                    $("#btn_submit").val("页面跳转中");
                    setTimeout(function () {
                        location.href = (returnUrl == "" || returnUrl == null ? "/Home/Index" : unescape(request("returnUrl")));
                    }, 500);
                }, 500);
            } else {
                if (data.Message) {
                    {
                        $("#btn_submit").val("登录");
                        dlg.msg.info(data.Message);
                        setTimeout(function () {
                            if (data.Message == "站点已过期") {
                                location.href = "/Login/Auth";
                            }
                        }, 2000);
                        self.code_changing();

                    }
                }
            }
        }
    });
});

function code_changing() {
    $("#ValidCode").attr("src", '/Login/ValidCode?aid=0925&rand=' + Math.random())
}