﻿/** layui-v1.0.4 LGPL-2.1 license By http://www.layui.com */
;
layui.define("jquery",
function (i) {
    "use strict";
    var t = layui.jquery,
    a = (layui.hint(), layui.device()),
    e = "element",
    n = "layui-this",
    l = "layui-show",
    s = function () {
        this.config = {}
    };
    s.prototype.set = function (i) {
        var a = this;
        return t.extend(!0, a.config, i),
        a
    },
    s.prototype.on = function (i, t) {
        return layui.onevent(e, i, t)
    },
    s.prototype.tabAdd = function (i, a) {
        var e = t(".layui-tab[lay-filter=" + i + "]"),
        n = e.children(".layui-tab-title"),
        l = e.children(".layui-tab-content");
        return n.append("<li>" + (a.title || "unnaming") + "</li>"),
        l.append('<div class="layui-tab-item">' + (a.content || "") + "</div>"),
        this.init()
    },
    s.prototype.tabDelete = function (i, a) {
        var e = t(".layui-tab[lay-filter=" + i + "]"),
        n = e.children(".layui-tab-title").find(">li").eq(a);
        return o.tabDelete(null, n),
        this
    },
    s.prototype.tabChange = function (i, a) {
        var e = t(".layui-tab[lay-filter=" + i + "]"),
        n = e.children(".layui-tab-title").find(">li").eq(a);
        return o.tabClick(null, a, n),
        this
    };
    var o = {
        tabClick: function (i, a, s) {
            var o = s || t(this),
            a = a || o.index(),
            u = o.parents(".layui-tab"),
            c = u.children(".layui-tab-content").children(".layui-tab-item"),
            r = u.attr("lay-filter");
            o.addClass(n).siblings().removeClass(n),
            c.eq(a).addClass(l).siblings().removeClass(l),
            layui.event.call(this, e, "tab(" + r + ")", {
                elem: u,
                index: a
            })
        },
        tabDelete: function (i, a) {
            var e = a || t(this).parent(),
            l = e.index(),
            s = e.parents(".layui-tab"),
            u = s.children(".layui-tab-content").children(".layui-tab-item");
            e.hasClass(n) && (e.next()[0] ? o.tabClick.call(e.next()[0], null, l + 1) : e.prev()[0] && o.tabClick.call(e.prev()[0], null, l - 1)),
            e.remove(),
            u.eq(l).remove()
        },
        tabAuto: function () {
            var i = "layui-tab-more",
            e = "layui-tab-bar",
            n = "layui-tab-close",
            l = this;
            t(".layui-tab").each(function () {
                var s = t(this),
                u = s.children(".layui-tab-title"),
                c = (s.children(".layui-tab-content").children(".layui-tab-item"), 'lay-stope="tabmore"'),
                r = t('<span class="layui-unselect layui-tab-bar" ' + c + "><i " + c + ' class="layui-icon">&#xe61a;</i></span>');
                if (l === window && 8 != a.ie && o.hideTabMore(!0), s.attr("lay-allowClose") && !u.find("li").find("." + n)[0]) {
                    var d = t('<i class="layui-icon layui-unselect ' + n + '">&#x1006;</i>');
                    d.on("click", o.tabDelete),
                    u.find("li").append(d)
                }
                if (u.prop("scrollWidth") > u.outerWidth() + 1) {
                    if (u.find("." + e)[0]) return;
                    u.append(r),
                    r.on("click",
                    function (t) {
                        u[this.title ? "removeClass" : "addClass"](i),
                        this.title = this.title ? "" : "鏀剁缉"
                    })
                } else u.find("." + e).remove()
            })
        },
        hideTabMore: function (i) {
            var a = t(".layui-tab-title");
            i !== !0 && "tabmore" === t(i.target).attr("lay-stope") || (a.removeClass("layui-tab-more"), a.find(".layui-tab-bar").attr("title", ""))
        }
    };
    s.prototype.init = function (i) {
        var e = {
            tab: function () {
                o.tabAuto.call({})
            },
            nav: function () {
                var i, e, n, l = ".layui-nav",
                s = "layui-nav-item",
                o = "layui-nav-bar",
                u = "layui-nav-tree",
                c = "layui-nav-child",
                r = "layui-nav-more",
                d = 200,
                y = function (l, s) {
                    var o = t(this),
                    y = o.find("." + c);
                    s.hasClass(u) ? l.css({
                        top: o.position().top,
                        height: o.children("a").height(),
                        opacity: 1
                    }) : (y.addClass("layui-anim layui-anim-upbit"), l.css({
                        left: o.position().left + parseFloat(o.css("marginLeft")),
                        top: o.position().top + o.height() - 5
                    }), i = setTimeout(function () {
                        l.css({
                            width: o.width(),
                            opacity: 1
                        })
                    },
                    a.ie && a.ie < 10 ? 0 : d), clearTimeout(n), "block" === y.css("display") && clearTimeout(e), e = setTimeout(function () {
                        y.show(),
                        o.find("." + r).addClass(r + "d")
                    },
                    300))
                };
                t(l).each(function () {
                    var a = t(this),
                    l = t('<span class="' + o + '"></span>'),
                    h = a.find("." + s);
                    a.find("." + o)[0] || (a.append(l), h.on("mouseenter",
                    function () {
                        y.call(this, l, a)
                    }).on("mouseleave",
                    function () {
                        a.hasClass(u) || (clearTimeout(e), e = setTimeout(function () {
                            a.find("." + c).hide(),
                            a.find("." + r).removeClass(r + "d")
                        },
                        300))
                    }), a.on("mouseleave",
                    function () {
                        clearTimeout(i),
                        n = setTimeout(function () {
                            a.hasClass(u) ? l.css({
                                height: 0,
                                top: l.position().top + l.height() / 2,
                                opacity: 0
                            }) : l.css({
                                width: 0,
                                left: l.position().left + l.width() / 2,
                                opacity: 0
                            })
                        },
                        d)
                    })),
                    h.each(function () {
                        var i = t(this),
                        e = i.find("." + c);
                        if (e[0] && !i.find("." + r)[0]) {
                            if (i.children("a").append('<span class="' + r + '"></span>'), !a.hasClass(u)) return;
                            i.children("a").on("click",
                            function () {
                                t(this);
                                "none" === e.css("display") ? i.addClass(s + "ed") : i.removeClass(s + "ed")
                            })
                        }
                    })
                })
            },
            breadcrumb: function () {
                var i = ".layui-breadcrumb";
                t(i).each(function () {
                    var i = t(this),
                    a = i.attr("lay-separator") || ">",
                    e = i.find("a");
                    e.find(".layui-box")[0] || (e.each(function (i) {
                        i !== e.length - 1 && t(this).append('<span class="layui-box">' + a + "</span>")
                    }), i.css("visibility", "visible"))
                })
            }
        };
        return layui.each(e,
        function (i, t) {
            t()
        })
    };
    var u = new s,
    c = t(document);
    u.init();
    var r = ".layui-tab-title li";
    c.on("click", r, o.tabClick),
    c.on("click", o.hideTabMore),
    t(window).on("resize", o.tabAuto),
    i(e,
    function (i) {
        return u.set(i)
    })
});