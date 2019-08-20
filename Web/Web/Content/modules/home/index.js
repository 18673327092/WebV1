var Web = {};

var menu = {
    init: function () {
        setInterval(function () {
            $(".main").css("height", ($(window).height() - 81) + "px");
            $(".menuContainer").css("height", ($(window).height() - 53) + "px");
        }, 500);
        $(window).resize(function () {
            $(".main").css("height", ($(window).height() - 81) + "px");
        });

        $(".p_menmu").click(function () {
             $(this).siblings("div").next("ul").hide(200);
            $(this).next("ul").toggle(200);
        });

        $("#userinfo").click(function () {
            $("#userinfoArrow").toggle();
        });
        $("#userinfo").hover(function () { }, function () {
            $("#userinfoArrow").hide();
        });
        $("#userinfoArrow").hover(function () {
            $(this).show();
        }, function () { $(this).hide(); });

        $(".headbar_nav li").mouseover(function () {
            $(".InstalList").hide();
            $(this).find(".InstalList").show();
        });
        $(".headbar_nav li").mouseout(function () {
            $(".InstalList").hide();
        });
        var menuid = parseInt(request("menuid")) || 0;
        if (menuid === 0) {
            $(".leftmenu:eq(0)").click();
        } else {
            $(".leftmenu[data-menuid=" + menuid + "]").click();
        }


        var _this = this;
        /* 鼠标移出左侧菜单的时候，隐藏二级菜单 */
        $(".leftMenu").hover(function () {// 对div的处理
            t = setTimeout(function () {
                _this.hide();
            }, 10);
        });
    },
    show: function (obj) {
        // 因为关闭二级菜单的时候存在延时关闭所二级菜单，所以在打开二级菜单的时候也需要延时打开，否则容易出现打开另外一个二级菜单之后调用关闭所有二级菜单的方法。
        t = setTimeout(function () {
            $("#firstmenu li").removeClass("select");
            // 隐藏二级菜单
            $(".secondMenu").hide();
            var secondmenu = $(obj).attr("secondmenu");
            $(obj).addClass("select");
            var clientHeight = window.document.body.clientHeight;
            var menuHeight = $("#" + secondmenu).css("height");
            menuHeight = menuHeight.substring(0, menuHeight.indexOf("px"));
            var top = $("#" + secondmenu).css("top");
            top = top.substring(0, top.indexOf("px"));
            if (clientHeight - menuHeight < top) {
                top = clientHeight - menuHeight - 20;
            }
            $("#" + secondmenu).css("top", top + "px");
            $("#" + secondmenu).show();


        }, 20);
    },
    hide: function () {
        $("#firstmenu li").removeClass("select");
        // 隐藏二级菜单
        $(".secondMenu").hide();
    },
    secondeHide: function (obj) {
        var _this = this;
        var showsecondmenu = ($(obj).attr("secondmenu"));
        if ($('#' + showsecondmenu).is(":visible")) {
            return;
        }
        t = setTimeout(function () {
            _this.hide();
        }, 10);
    },
    clearTime: function () {
        clearTimeout(t);
    }
};


$(function () {
    menu.init();
});


function checkAndAddTab(obj) {
    tab.add(obj);
    $(".sub_menu>li").removeClass("active");
    $(".menmu").removeClass("active");
    $(obj).addClass("active");
}

var logintime = setInterval(function () {
    $.get("/Login/IsLogin", {}, function (data) {
        if (!data.Success) {
            clearInterval(logintime);
            location.href = "/Login?code=-1";
        }
    })
}, 5000)

function LoginOut() {
    dlg.confirm("确定退出系统吗？", function () {
        location.href = "/Login/LoginOut";
    })
}

function UpdatePW() {
    dlg.openframe({
        offset: ['80px'],
        title: "修改密码",
        content: "/User/UpdatePassword",
        area: ['500px', '300px;']
    })
}