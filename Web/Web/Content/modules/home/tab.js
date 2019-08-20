var tab = {
    add: function (obj) {
        var _this = this;
        var menuid = $(obj).data("menuid");
        var icon = $(obj).data("icon");
        // tab id 以该页面url定义
        var url = $(obj).data("basepath") + $(obj).data("url");
        var tabid = $(obj).data("url").replace(".htm", "").replace(/\//g, "-");
        if (tabid.indexOf("?") > 0) {
            tabid = tabid.substring(0, tabid.indexOf("?"));
        }
        // 获取tab的标题
        var title = "";
        if ($(obj).attr("title") != undefined) {
            title = $(obj).attr("title");
        } else {
            title = $(obj).html();
        }
        if (title.indexOf("<") != -1) {// 特殊处理菜单中添加的new标志span，不能出现在标题中
            title = title.substr(0, title.indexOf("<"));
        }
        // 隐藏二级菜单
        $(".secondMenu").hide();
        if (($(".scrol").find("li").length * 115) > $(".topTab").width()) {
            _this.dele($(".scrol").find("li:eq(1)"))
        }
        _this.open(title, url, menuid, false, icon);
    },    // 通过菜单新增tab
    open: function (title, url, tabid, isRefresh, icon) {
        var random = Math.random();
        // 增加随机数解决 firefox 不能刷新问题
        // url = url.indexOf("?") > -1 ? (url + "&random=" + random) : (url + "?random=" + random);

        // 如果点击了已打开的tab
        if ($("#" + tabid).length > 0) {
            if (url.indexOf("?") > -1) {
                url = url + '&tabid=' + tabid;
            } else {
                url = url + '?tabid=' + tabid;
            }
            var index = -1;
            // 移除tab中样式
            $("#tab li").each(function (i) {
                $(this).attr("class", "normal");
                if (this.id == tabid) {
                    index = i;
                }
            });
            $("#" + tabid).attr("class", "select");
            $("#" + tabid).find(".title").html(title);
            if (index != -1) {
                $(".main").hide();
                $($(".main")[index]).show();
                if (isRefresh) {
                    $("#" + tabid).attr("url", url);
                    $($(".main")[index]).find("#iframe")[0].contentWindow.location.href = url;
                }
            }
        } else {
            // 获取tab的链接
            var html = "";
            if (url.indexOf("?") > -1) {
                html = url + '&tabid=' + tabid;
            } else {
                html = url + '?tabid=' + tabid;
            }

            // 移除tab中样式
            $("#tab li").each(function () {
                $(this).attr("class", "normal");
            });
            // var copyHome = $("#tab li:eq(0)").clone();
            var copyHome = $('<li  class="select"></li>');
            copyHome.attr("id", tabid);
            copyHome.attr("tabid", tabid);
            copyHome.attr("class", "select");
            copyHome.attr("url", html);
            if (typeof icon !== "undefined") {
                copyHome.html('<span class="icon icon_hy1 fl fa fa-' + icon + ' " style=" margin: 5px 4px 0px -5px;"></span><span  class="fl title" title="'
                    + title
                    + '" url="'
                    + html
                    + '" href="javascript:;" tabid="'
                    + tabid
                    + '" onclick="tab.select(this)">'
                    + title
                    + '</span><a tabid="'
                    + tabid
                    + '" class="tabClose" href="javascript:;" onclick="tab.dele(this)"></a>');
            } else {
                copyHome.html('<span  class="fl title" title="'
                    + title
                    + '" url="'
                    + html
                    + '" href="javascript:;" tabid="'
                    + tabid
                    + '" onclick="tab.select(this)">'
                    + title
                    + '</span><a tabid="'
                    + tabid
                    + '" class="tabClose" href="javascript:;" onclick="tab.dele(this)"></a>');
            }

            $("#tab").append(copyHome);
            var copyMainPage = $("#maincontentinit").clone();
            copyMainPage.addClass("page");
            copyMainPage.attr("id", "maincontent");
            copyMainPage.attr("name", "maincontent");
            copyMainPage.css('overflow', 'hidden');
            $("#mainpage").append(copyMainPage);
            $(".main").hide();
            copyMainPage.find("#iframe").attr("src", html).css({ "height": "100%" });
            copyMainPage.css({ "height": "100%" });
            copyMainPage.show();
        }
    },   // isRefresh 是否刷新
    select: function (obj, isrefresh) {
        $("#firstmenu li").each(function () {
            $(this).removeClass("select");
        });
        var title = $(obj).attr("title");
        var tabid = $(obj).attr("tabid");
        $("#" + tabid).addClass("select");
        $(".sub_menu>li").removeClass("active");
        $(".menuContainer>div").removeClass("active");
        $("[data-menuid=" + tabid + "]").addClass("active");
        // 移除tab中样式
        var index = 0;
        $("#tab li").each(function (i) {
            $(this).attr("class", "normal");
            if (this.id == tabid) {
                index = i;
            }
        });
        if (tabid == undefined) {
            $("#home").attr("class", "select");
        } else {
            $("#" + tabid).attr("class", "select");
        }

        $(".main").hide();
        $($(".main")[index]).show();
        if (isrefresh) {
            $($(".main")[index]).find("#iframe")[0].contentWindow.location.reload();
        }
    },
    dele: function (obj) {
        var _this = this;
        $(".loadding").hide();
        var tabid = $(obj).attr("tabid");
        if (tabid.indexOf("desktop-newstart") >= 0 && getCookie("isshow_newstart") == "true") {
            showErrorMsg("请先将新手引导从标签页解锁");
            return;
        }
        if ('InvTak-Add' == tabid && _this.isExist('InvTak-ImportInit')) {
            _this.deleById('InvTak-ImportInit');
        }
        _this.deleById(tabid);
    },  // 删除tab
    deleById: function (tabid, isSelect, se_tabid) {
        var _this = this;
        if (tabid == "" || tabid == null || tabid == undefined) {
            return;
        }
        if ($("#tab li[id$='" + tabid + "']").attr("locktab")) {
            selectTab($("#tab li[id='businessprocess-stepone']"));
            showErrorMsg("请先解除锁定后关闭");
            return;
        }
        var index = -1;
        $("#tab li").each(
            function (i) {
                if (isSelect == undefined || isSelect == null || isSelect) {
                    $(this).attr("class", "normal");
                }
                var id = this.id;
                if (id.indexOf(tabid) > -1 && id.substring(id.indexOf(tabid)) == tabid) {
                    index = i;
                }
            });
        $("#tab li[id$='" + tabid + "']").remove();
        if (index > -1) {
            $($(".main")[index]).remove();
        }

        if (se_tabid) {
            if (se_tabid.indexOf("-") !== -1) {
                se_tabid = se_tabid.substring(se_tabid.indexOf("-") + 1);
            }
            if (se_tabid.indexOf("-") !== -1) {
                se_tabid = se_tabid.substring(0, se_tabid.indexOf("-"));
            }
            _this.select($("#tab").find('li[tabid=' + se_tabid + ']'));
        }
        else if (isSelect == undefined || isSelect == null || isSelect) {
            _this.select($("#tab li:last-child"));
        }
    },  // isSelect 表示是否需要选中最后一个tab
    deleByName: function (tabname) {
        var title = null;
        var index = -1;
        $("#tab li").each(function (i) {
            title = $(this).find("span").html();
            if (title == tabname) {
                $(this).remove();
                index = i;
            }
        });
        if (index > -1) {
            $($(".main")[index]).remove();
        }
    },
    isExist: function (tabid) {
        var flag = false;
        $("#tab li").each(function (i) {
            if (this.id == tabid) {
                flag = true;
            }
        });
        return flag;
    },//根据tabid判断tab是否存在
    get: function (tabid) {
        var j = 0;
        $(document).find("#tabDiv li").each(function (i) {
            if ($(this).attr("tabid") == tabid) {
                j = i;
            }
        });

        var index = 0;
        $("#tab li").each(function (i) {
            $(this).attr("class", "normal");
            if (this.id == tabid) {
                index = i;
            }
        });
        if (tabid == undefined) {
            $("#home").attr("class", "select");
        } else {
            $("#" + tabid).attr("class", "select");
        }
        $(".main").hide();
        $($(".main")[index]).show();

        return $($(document).find(".main")[j]).find("#iframe")[0].contentWindow;
    },
    refresh: function (url) {
        if (url != undefined && url != null) {
            $(".main:visible").find("#iframe")[0].contentWindow.location.href = url;
        } else {
            var src = $($(".main:visible").find("#iframe")[0]).attr("src");
            if (undefined != src && src.indexOf("pointExchange/itemList.htm") > 0) {
                $($(".main:visible").find("#iframe")[0]).attr("src", src);//通过src刷新页面
            } else
                $(".main:visible").find("#iframe")[0].contentWindow.location.reload();
        }
    },// 刷新指定url的tab
    refreshByName: function (tabName) {
        var j = "";
        $("#tab li").each(function (i) {
            var title = $(this).find("span").attr("title");
            if ($.trim(title) == tabName) {
                j = i;
            }
        });
        if (j != "") {
            $(".main").find("#iframe")[j].contentWindow.location.reload();
        }
    },// 刷新指定name的tab
    refreshById: function (tabid) {
        var j = "";
        $("#tab li").each(function (i) {
            var curtabid = $(this).find("span").attr("tabid");
            if ($.trim(curtabid) == tabid) {
                j = i;
            }
        });
        if (j != "") {
            $(".main").find("#iframe")[j].contentWindow.location.href = $(".main")
                .find("#iframe")[j].contentWindow.location.href;
        }
    },// 刷新指定tabid的tab
    refreshAll: function () {
        $("#tab li").each(function (i) {
            $(".main").find("#iframe")[i].contentWindow.location.reload();
        });
    },
    closeAll: function (tabid) {
        $(".loadding").hide();
        var len = $("#mainpage .main").length;
        $("#tab li").each(
            function (i) {
                var id = $(this).attr("id");
                // var isremove = true;
                if (id != "home" && !$(this).attr("locktab")) {
                    $(this).remove();
                }
                if (len != 0 && !$($(".main")[len]).attr("locktab")
                    && $($(".main")[len]).attr("id") != "maincontentinit") {
                    $($(".main")[len]).remove();
                }
                len = len - 1;
                $(this).attr("class", "normal");
            });

        $("#home").attr("class", "select");
        $($(".main")[0]).show();

        // 恢复tab行到首页，防止值一个菜单是首页被隐藏
        $(".u .scrol").css({
            left: "0px"
        });
    },// 关闭全部tab
    closeOther: function () {
        $(".loadding").hide();
        $("#tab li").each(
            function (i) {
                var id = $(this).attr("id");
                if (id != "home" && $(this).attr("class") != "select"
                    && !$(this).attr("locktab")) {
                    $(this).remove();
                }
            });
        $(".main").each(
            function (i) {
                if ($(this).hasClass('page')
                    && $(this).css("display") == "none"
                    && !$(this).attr("locktab")) {
                    $(this).remove();
                }
            });
        // 恢复tab行到首页，防止值一个菜单是首页被隐藏
        $(".u .scrol").css({
            left: "0px"
        });
    },// 关闭其他
}