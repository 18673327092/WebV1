//调整iframe高度
function reinitIframe() {
    var iframes = document.getElementsByName("iframe");
    try {
        for (var i = 0; i < iframes.length; i++) {
            //iframes[i].height = iframes[i].contentWindow.document.documentElement.scrollHeight;
            iframes[i].height = $(window).height() - 90;
        }


    } catch (ex) { }
}
window.setInterval("reinitIframe()", 200);

layui.use('element', function () {
    var $ = layui.jquery, element = layui.element(); //Tab的切换功能，切换事件监听等，需要依赖element模块
    //触发事件
    var active = {
        tabAdd: function () {
            var icon = $(this).data("icon");
            var title = $(this).text().trim() + "<i class='layui-icon layui-unselect layui-tab-close'>ဆ</i>";
            if (icon != "") {
                title = "<i class='fa " + icon + "'></i> " + title;
            }
            //新增一个Tab项
            element.tabAdd('menu', {
                title: title,
                content: '<iframe name="iframe" src="' + $(this).data("url") + '" frameborder="0" height="0"; style="width: 100%;    margin-bottom: -5px;"></iframe>'
            });
            if ($("#tabTitle").find("span.layui-unselect.layui-tab-bar").length > 0) {
                element.tabDelete("menu", 1);
            }
            //增加点击关闭事件
            var r = $("#tabTitle").find("li");
            //每次新打开tab都是最后一个，所以只对最后一个tab添加点击关闭事件
            r.eq(r.length - 1).children("i.layui-tab-close").on("click", function () {
                element.tabDelete("menu", $(this).parent("li").index());
            }), element.tabChange("menu", r.length - 1);
            element.init();
        },
        tabDelete: function (index) {
            //删除指定Tab项
            element.tabDelete('menu', index); //删除（注意序号是从0开始计算）
        },
        tabChange: function (index) {
            //切换到指定Tab项
            element.tabChange('menu', index); //切换（注意序号是从0开始计算）
        }
    };

    $("a[name='menuurl']").on("click", function () {
        var title = $(this).text();
        var tabs = $("#tabTitle").children();
       
        for (var i = 0; i < tabs.length; i++) {
            var _tab = $(tabs[i]).clone()
            var _taba = _tab.find("i").remove();
            if (_tab.text().trim() == title.trim()) {
                element.tabChange('menu', i);
                return;
            }
        }
        active["tabAdd"].call(this);
        active.tabChange($("#tabTitle").children().length - 1);
    });
    $("a[name='menuurl']:eq(0)").click();
    $("#tabTitle").find("li:eq(0)").find("i:eq(1)").hide();
});


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

$(function () {

});