﻿(function (b) {
    b.validator.config(
        {
            defaultMsg: "{0}格式不正确",
            loadingMsg: "正在验证...",
            rules: {
                digits: [/^\d+$/, "请输入数字"],
                letters: [/^[a-z]+$/i, "{0}只能输入字母"],
                tel: [/^(?:(?:0\d{2,3}[- ]?[1-9]\d{6,7})|(?:[48]00[- ]?[1-9]\d{6}))$/, "电话格式不正确"],
                mobile: [/^1\d{10}$/, "手机号格式不正确"],
                email: [/^[\w\+-]+(\.[\w\+-]+)*@[a-z\d-]+(\.[a-z\d-]+)*\.([a-z]{2,4})$/i, "邮箱格式不正确"],
                qq: [/^[1-9]\d{4,}$/, "QQ号格式不正确"], date: [/^\d{4}-\d{1,2}-\d{1,2}$/, "请输入正确的日期,例:yyyy-mm-dd"],
                time: [/^([01]\d|2[0-3])(:[0-5]\d){1,2}$/, "请输入正确的时间,例:14:30或14:30:00"],
                ID_card: [/^[1-9]\d{5}[1-9]\d{3}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])((\d{4})|\d{3}[A-Z])$/, "请输入正确的身份证号码"],
                url: [/^(https?|ftp):\/\/[^\s]+$/i, "网址格式不正确"],
                postcode: [/^[1-9]\d{5}$/, "邮编格式不正确"],
                chinese: [/^[\u0391-\uFFE5]+$/, "请输入中文"],
                username: [/^\w{3,12}$/, "请输入3-12位数字、字母、下划线"],
                loginname: [/^\w*[a-zA-Z]+\w*$/, "用户名必须为长度为6-20的字母或字母与数字的组合"],
                password: [/^[\@A-Za-z0-9\!\#\$\%\^\&\*\.\~\=\+]{6,20}$/, "登录密码长度必须为6-20个字符"],
                tel_mobile: [/^((?:(?:0\d{2,3}[- ]?[1-9]\d{6,7})|(?:[48]00[- ]?[1-9]\d{6})))|(1\d{10})$/, "电话号码格式不正确"],
                number: [/^[+-]?\d+(\.\d+)?$/, "请输入数字"],
                name: [/^[\u4E00-\u9FA5A-Za-z0-9_]+$/, "只能输入中文,字母数字下划线或横杠"],
                code: [/^[A-Za-z0-9_-]+$/, "编号必须为字母、数字、横杠或下划线"],
                numbercode: [/^\w*[a-zA-Z]\w*$/, "用户名必须为字母和数字的组合"],
                mobileoremail: [/^((1\d{10})|([\w\+-]+(\.[\w\+-]+)*@[a-z\d-]+(\.[a-z\d-]+)*\.([a-z]{2,4})))$/, "手机号或邮箱格式不正确"],
                accept: function (c, e) { if (!e) { return true; } var d = e[0]; return (d === "*") || (new RegExp(".(?:" + (d || "png|jpg|jpeg|gif") + ")$", "i")).test(c.value) || this.renderMsg("只接受{1}后缀", d.replace("|", ",")); }
            }
        }); b.validator.config(
            {
                messages: {
                    required: "{0}不能为空",
                    remote: "{0}已被使用",
                    integer: { "*": "请输入整数", "+": "请输入正整数", "+0": "请输入正整数或0", "-": "请输入负整数", "-0": "请输入负整数或0" },
                    match: { eq: "{0}与{1}不一致", neq: "{0}与{1}不能相同", lt: "{0}必须小于{1}", gt: "{0}必须大于{1}", lte: "{0}必须小于或等于{1}", gte: "{0}必须大于或等于{1}" }, range: { rg: "请输入{1}到{2}的数", gt: "请输入大于或等于{1}的数", lt: "请输入小于或等于{1}的数" }, checked: { eq: "请选择{1}项", rg: "请选择{1}到{2}项", gt: "请至少选择{1}项", lt: "请最多选择{1}项" }, length: { eq: "请输入{1}个字符", rg: "请输入{1}到{2}个字符", gt: "请至少输入{1}个字符", lt: "请最多输入{1}个字符", eq_2: "", rg_2: "", gt_2: "", lt_2: "" }
                }
            });
    var a = '<span class="n-arrow"><b>◆</b><i>◆</i></span>';
    b.validator.setTheme(
        {
            simple_right: {
                formClass: "n-simple", msgClass: "n-right"
            }, simple_bottom:
                {
                    formClass: "n-simple",
                    msgClass: "n-bottom"
                }, yellow_top:
                    {
                        formClass: "n-yellow",
                        msgClass: "n-top", msgArrow: a
                    }, yellow_right: {
                        formClass: "n-yellow",
                        msgClass: "n-right", msgArrow: a
                    },
            yellow_right_effect:
                {
                    formClass: "n-yellow", msgClass: "n-right",
                    msgArrow: a,
                    msgShow: function (e, d) {
                        var c = e.children();
                        if (c.is(":animated")) { return; } if (d === "error") { c.css({ left: "20px", opacity: 0 }).delay(100).show().stop().animate({ left: "-4px", opacity: 1 }, 150).animate({ left: "3px" }, 80).animate({ left: 0 }, 80); } else { c.css({ left: 0, opacity: 1 }).fadeIn(200); }
                    },
                    msgHide: function (e, d) { var c = e.children(); c.stop().delay(100).show().animate({ left: "20px", opacity: 0 }, 300, function () { e.hide(); }); }
                }
        });
})(jQuery);