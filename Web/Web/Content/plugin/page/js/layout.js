jQuery.cookie = function (name, value, options) {
    if (typeof value != "undefined") {
        options = options || {};
        if (value === null) {
            value = "";
            options.expires = -1
        }
        var expires = "";
        if (options.expires && (typeof options.expires == "number" || options.expires.toUTCString)) {
            var date;
            if (typeof options.expires == "number") {
                date = new Date();
                date.setTime(date.getTime() + (options.expires * 24 * 60 * 60 * 1000))
            } else {
                date = options.expires
            }
            expires = "; expires=" + date.toUTCString()
        }
        var path = options.path ? "; path=" + (options.path) : "";
        var domain = options.domain ? "; domain=" + (options.domain) : "";
        var secure = options.secure ? "; secure" : "";
        document.cookie = [name, "=", encodeURIComponent(value), expires, path, domain, secure].join("")
    } else {
        var cookieValue = null;
        if (document.cookie && document.cookie != "") {
            var cookies = document.cookie.split(";");
            for (var i = 0; i < cookies.length; i++) {
                var cookie = jQuery.trim(cookies[i]);
                if (cookie.substring(0, name.length + 1) == (name + "=")) {
                    cookieValue = decodeURIComponent(cookie.substring(name.length + 1));
                    break
                }
            }
        }
        return cookieValue
    }
};
/*! iCheck v1.0.2 by Damir Sultanov, http://git.io/arlzeA, MIT Licensed */
(function (f) {
    function A(a, b, d) {
        var c = a[0],
        g = /er/.test(d) ? _indeterminate : /bl/.test(d) ? n : k,
        e = d == _update ? {
            checked: c[k],
            disabled: c[n],
            indeterminate: "true" == a.attr(_indeterminate) || "false" == a.attr(_determinate)
        } : c[g];
        if (/^(ch|di|in)/.test(d) && !e) {
            x(a, g)
        } else {
            if (/^(un|en|de)/.test(d) && e) {
                q(a, g)
            } else {
                if (d == _update) {
                    for (var f in e) {
                        e[f] ? x(a, f, !0) : q(a, f, !0)
                    }
                } else {
                    if (!b || "toggle" == d) {
                        if (!b) {
                            a[_callback]("ifClicked")
                        }
                        e ? c[_type] !== r && q(a, g) : x(a, g)
                    }
                }
            }
        }
    }
    function x(a, b, d) {
        var c = a[0],
        g = a.parent(),
        e = b == k,
        u = b == _indeterminate,
        v = b == n,
        s = u ? _determinate : e ? y : "enabled",
        F = l(a, s + t(c[_type])),
        B = l(a, b + t(c[_type]));
        if (!0 !== c[b]) {
            if (!d && b == k && c[_type] == r && c.name) {
                var w = a.closest("form"),
                p = 'input[name="' + c.name + '"]',
                p = w.length ? w.find(p) : f(p);
                p.each(function () {
                    this !== c && f(this).data(m) && q(f(this), b)
                })
            }
            u ? (c[b] = !0, c[k] && q(a, k, "force")) : (d || (c[b] = !0), e && c[_indeterminate] && q(a, _indeterminate, !1));
            D(a, e, b, d)
        }
        c[n] && l(a, _cursor, !0) && g.find("." + C).css(_cursor, "default");
        g[_add](B || l(a, b) || "");
        g.attr("role") && !u && g.attr("aria-" + (v ? n : k), "true");
        g[_remove](F || l(a, s) || "")
    }
    function q(a, b, d) {
        var c = a[0],
        g = a.parent(),
        e = b == k,
        f = b == _indeterminate,
        m = b == n,
        s = f ? _determinate : e ? y : "enabled",
        q = l(a, s + t(c[_type])),
        r = l(a, b + t(c[_type]));
        if (!1 !== c[b]) {
            if (f || !d || "force" == d) {
                c[b] = !1
            }
            D(a, e, s, d)
        } !c[n] && l(a, _cursor, !0) && g.find("." + C).css(_cursor, "pointer");
        g[_remove](r || l(a, b) || "");
        g.attr("role") && !f && g.attr("aria-" + (m ? n : k), "false");
        g[_add](q || l(a, s) || "")
    }
    function E(a, b) {
        if (a.data(m)) {
            a.parent().html(a.attr("style", a.data(m).s || ""));
            if (b) {
                a[_callback](b)
            }
            a.off(".i").unwrap();
            f(_label + '[for="' + a[0].id + '"]').add(a.closest(_label)).off(".i")
        }
    }
    function l(a, b, f) {
        if (a.data(m)) {
            return a.data(m).o[b + (f ? "" : "Class")]
        }
    }
    function t(a) {
        return a.charAt(0).toUpperCase() + a.slice(1)
    }
    function D(a, b, f, c) {
        if (!c) {
            if (b) {
                a[_callback]("ifToggled")
            }
            a[_callback]("ifChanged")[_callback]("if" + t(f))
        }
    }
    var m = "iCheck",
    C = m + "-helper",
    r = "radio",
    k = "checked",
    y = "un" + k,
    n = "disabled";
    _determinate = "determinate";
    _indeterminate = "in" + _determinate;
    _update = "update";
    _type = "type";
    _click = "click";
    _touch = "touchbegin.i touchend.i";
    _add = "addClass";
    _remove = "removeClass";
    _callback = "trigger";
    _label = "label";
    _cursor = "cursor";
    _mobile = /ipad|iphone|ipod|android|blackberry|windows phone|opera mini|silk/i.test(navigator.userAgent);
    f.fn[m] = function (a, b) {
        var d = 'input[type="checkbox"], input[type="' + r + '"]',
        c = f(),
        g = function (a) {
            a.each(function () {
                var a = f(this);
                c = a.is(d) ? c.add(a) : c.add(a.find(d))
            })
        };
        if (/^(check|uncheck|toggle|indeterminate|determinate|disable|enable|update|destroy)$/i.test(a)) {
            return a = a.toLowerCase(),
            g(this),
            c.each(function () {
                var c = f(this);
                "destroy" == a ? E(c, "ifDestroyed") : A(c, !0, a);
                f.isFunction(b) && b()
            })
        }
        if ("object" != typeof a && a) {
            return this
        }
        var e = f.extend({
            checkedClass: k,
            disabledClass: n,
            indeterminateClass: _indeterminate,
            labelHover: !0
        },
        a),
        l = e.handle,
        v = e.hoverClass || "hover",
        s = e.focusClass || "focus",
        t = e.activeClass || "active",
        B = !!e.labelHover,
        w = e.labelHoverClass || "hover",
        p = ("" + e.increaseArea).replace("%", "") | 0;
        if ("checkbox" == l || l == r) {
            d = 'input[type="' + l + '"]'
        } -50 > p && (p = -50);
        g(this);
        return c.each(function () {
            var a = f(this);
            E(a);
            var c = this,
            b = c.id,
            g = -p + "%",
            d = 100 + 2 * p + "%",
            d = {
                position: "absolute",
                top: g,
                left: g,
                display: "block",
                width: d,
                height: d,
                margin: 0,
                padding: 0,
                background: "#fff",
                border: 0,
                opacity: 0
            },
            g = _mobile ? {
                position: "absolute",
                visibility: "hidden"
            } : p ? d : {
                position: "absolute",
                opacity: 0
            },
            l = "checkbox" == c[_type] ? e.checkboxClass || "icheckbox" : e.radioClass || "i" + r,
            z = f(_label + '[for="' + b + '"]').add(a.closest(_label)),
            u = !!e.aria,
            y = m + "-" + Math.random().toString(36).substr(2, 6),
            h = '<div class="' + l + '" ' + (u ? 'role="' + c[_type] + '" ' : "");
            u && z.each(function () {
                h += 'aria-labelledby="';
                this.id ? h += this.id : (this.id = y, h += y);
                h += '"'
            });
            h = a.wrap(h + "/>")[_callback]("ifCreated").parent().append(e.insert);
            d = f('<ins class="' + C + '"/>').css(d).appendTo(h);
            a.data(m, {
                o: e,
                s: a.attr("style")
            }).css(g);
            e.inheritClass && h[_add](c.className || "");
            e.inheritID && b && h.attr("id", m + "-" + b);
            "static" == h.css("position") && h.css("position", "relative");
            A(a, !0, _update);
            if (z.length) {
                z.on(_click + ".i mouseover.i mouseout.i " + _touch,
                function (b) {
                    var d = b[_type],
                    e = f(this);
                    if (!c[n]) {
                        if (d == _click) {
                            if (f(b.target).is("a")) {
                                return
                            }
                            A(a, !1, !0)
                        } else {
                            B && (/ut|nd/.test(d) ? (h[_remove](v), e[_remove](w)) : (h[_add](v), e[_add](w)))
                        }
                        if (_mobile) {
                            b.stopPropagation()
                        } else {
                            return !1
                        }
                    }
                })
            }
            a.on(_click + ".i focus.i blur.i keyup.i keydown.i keypress.i",
            function (b) {
                var d = b[_type];
                b = b.keyCode;
                if (d == _click) {
                    return !1
                }
                if ("keydown" == d && 32 == b) {
                    return c[_type] == r && c[k] || (c[k] ? q(a, k) : x(a, k)),
                    !1
                }
                if ("keyup" == d && c[_type] == r) {
                    !c[k] && x(a, k)
                } else {
                    if (/us|ur/.test(d)) {
                        h["blur" == d ? _remove : _add](s)
                    }
                }
            });
            d.on(_click + " mousedown mouseup mouseover mouseout " + _touch,
            function (b) {
                var d = b[_type],
                e = /wn|up/.test(d) ? t : v;
                if (!c[n]) {
                    if (d == _click) {
                        A(a, !1, !0)
                    } else {
                        if (/wn|er|in/.test(d)) {
                            h[_add](e)
                        } else {
                            h[_remove](e + " " + t)
                        }
                        if (z.length && B && e == v) {
                            z[/ut|nd/.test(d) ? _remove : _add](w)
                        }
                    }
                    if (_mobile) {
                        b.stopPropagation()
                    } else {
                        return !1
                    }
                }
            })
        })
    }
})(window.jQuery || window.Zepto);
if (!jQuery) {
    throw new Error("Bootstrap requires jQuery")
} +
function (a) {
    function b() {
        var a = document.createElement("bootstrap"),
        b = {
            WebkitTransition: "webkitTransitionEnd",
            MozTransition: "transitionend",
            OTransition: "oTransitionEnd otransitionend",
            transition: "transitionend"
        };
        for (var c in b) {
            if (void 0 !== a.style[c]) {
                return {
                    end: b[c]
                }
            }
        }
    }
    a.fn.emulateTransitionEnd = function (b) {
        var c = !1,
        d = this;
        a(this).one(a.support.transition.end,
        function () {
            c = !0
        });
        var e = function () {
            c || a(d).trigger(a.support.transition.end)
        };
        return setTimeout(e, b),
        this
    },
    a(function () {
        a.support.transition = b()
    })
}(window.jQuery),
+
function (a) {
    var b = '[data-dismiss="alert"]',
    c = function (c) {
        a(c).on("click", b, this.close)
    };
    c.prototype.close = function (b) {
        function c() {
            f.trigger("closed.bs.alert").remove()
        }
        var d = a(this),
        e = d.attr("data-target");
        e || (e = d.attr("href"), e = e && e.replace(/.*(?=#[^\s]*$)/, ""));
        var f = a(e);
        b && b.preventDefault(),
        f.length || (f = d.hasClass("alert") ? d : d.parent()),
        f.trigger(b = a.Event("close.bs.alert")),
        b.isDefaultPrevented() || (f.removeClass("in"), a.support.transition && f.hasClass("fade") ? f.one(a.support.transition.end, c).emulateTransitionEnd(150) : c())
    };
    var d = a.fn.alert;
    a.fn.alert = function (b) {
        return this.each(function () {
            var d = a(this),
            e = d.data("bs.alert");
            e || d.data("bs.alert", e = new c(this)),
            "string" == typeof b && e[b].call(d)
        })
    },
    a.fn.alert.Constructor = c,
    a.fn.alert.noConflict = function () {
        return a.fn.alert = d,
        this
    },
    a(document).on("click.bs.alert.data-api", b, c.prototype.close)
}(window.jQuery),
+
function (a) {
    var b = function (c, d) {
        this.$element = a(c),
        this.options = a.extend({},
        b.DEFAULTS, d)
    };
    b.DEFAULTS = {
        loadingText: "loading..."
    },
    b.prototype.setState = function (a) {
        var b = "disabled",
        c = this.$element,
        d = c.is("input") ? "val" : "html",
        e = c.data();
        a += "Text",
        e.resetText || c.data("resetText", c[d]()),
        c[d](e[a] || this.options[a]),
        setTimeout(function () {
            "loadingText" == a ? c.addClass(b).attr(b, b) : c.removeClass(b).removeAttr(b)
        },
        0)
    },
    b.prototype.toggle = function () {
        var a = this.$element.closest('[data-toggle="buttons"]');
        if (a.length) {
            var b = this.$element.find("input").prop("checked", !this.$element.hasClass("active")).trigger("change");
            "radio" === b.prop("type") && a.find(".active").removeClass("active")
        }
        this.$element.toggleClass("active")
    };
    var c = a.fn.button;
    a.fn.button = function (c) {
        return this.each(function () {
            var d = a(this),
            e = d.data("bs.button"),
            f = "object" == typeof c && c;
            e || d.data("bs.button", e = new b(this, f)),
            "toggle" == c ? e.toggle() : c && e.setState(c)
        })
    },
    a.fn.button.Constructor = b,
    a.fn.button.noConflict = function () {
        return a.fn.button = c,
        this
    },
    a(document).on("click.bs.button.data-api", "[data-toggle^=button]",
    function (b) {
        var c = a(b.target);
        c.hasClass("btn") || (c = c.closest(".btn")),
        c.button("toggle"),
        b.preventDefault()
    })
}(window.jQuery),
+
function (a) {
    var b = function (b, c) {
        this.$element = a(b),
        this.$indicators = this.$element.find(".carousel-indicators"),
        this.options = c,
        this.paused = this.sliding = this.interval = this.$active = this.$items = null,
        "hover" == this.options.pause && this.$element.on("mouseenter", a.proxy(this.pause, this)).on("mouseleave", a.proxy(this.cycle, this))
    };
    b.DEFAULTS = {
        interval: 5000,
        pause: "hover",
        wrap: !0
    },
    b.prototype.cycle = function (b) {
        return b || (this.paused = !1),
        this.interval && clearInterval(this.interval),
        this.options.interval && !this.paused && (this.interval = setInterval(a.proxy(this.next, this), this.options.interval)),
        this
    },
    b.prototype.getActiveIndex = function () {
        return this.$active = this.$element.find(".item.active"),
        this.$items = this.$active.parent().children(),
        this.$items.index(this.$active)
    },
    b.prototype.to = function (b) {
        var c = this,
        d = this.getActiveIndex();
        return b > this.$items.length - 1 || 0 > b ? void 0 : this.sliding ? this.$element.one("slid",
        function () {
            c.to(b)
        }) : d == b ? this.pause().cycle() : this.slide(b > d ? "next" : "prev", a(this.$items[b]))
    },
    b.prototype.pause = function (b) {
        return b || (this.paused = !0),
        this.$element.find(".next, .prev").length && a.support.transition.end && (this.$element.trigger(a.support.transition.end), this.cycle(!0)),
        this.interval = clearInterval(this.interval),
        this
    },
    b.prototype.next = function () {
        return this.sliding ? void 0 : this.slide("next")
    },
    b.prototype.prev = function () {
        return this.sliding ? void 0 : this.slide("prev")
    },
    b.prototype.slide = function (b, c) {
        var d = this.$element.find(".item.active"),
        e = c || d[b](),
        f = this.interval,
        g = "next" == b ? "left" : "right",
        h = "next" == b ? "first" : "last",
        i = this;
        if (!e.length) {
            if (!this.options.wrap) {
                return
            }
            e = this.$element.find(".item")[h]()
        }
        this.sliding = !0,
        f && this.pause();
        var j = a.Event("slide.bs.carousel", {
            relatedTarget: e[0],
            direction: g
        });
        if (!e.hasClass("active")) {
            if (this.$indicators.length && (this.$indicators.find(".active").removeClass("active"), this.$element.one("slid",
            function () {
                var b = a(i.$indicators.children()[i.getActiveIndex()]);
                b && b.addClass("active")
            })), a.support.transition && this.$element.hasClass("slide")) {
                if (this.$element.trigger(j), j.isDefaultPrevented()) {
                    return
                }
                e.addClass(b),
                e[0].offsetWidth,
                d.addClass(g),
                e.addClass(g),
                d.one(a.support.transition.end,
                function () {
                    e.removeClass([b, g].join(" ")).addClass("active"),
                    d.removeClass(["active", g].join(" ")),
                    i.sliding = !1,
                    setTimeout(function () {
                        i.$element.trigger("slid")
                    },
                    0)
                }).emulateTransitionEnd(600)
            } else {
                if (this.$element.trigger(j), j.isDefaultPrevented()) {
                    return
                }
                d.removeClass("active"),
                e.addClass("active"),
                this.sliding = !1,
                this.$element.trigger("slid")
            }
            return f && this.cycle(),
            this
        }
    };
    var c = a.fn.carousel;
    a.fn.carousel = function (c) {
        return this.each(function () {
            var d = a(this),
            e = d.data("bs.carousel"),
            f = a.extend({},
            b.DEFAULTS, d.data(), "object" == typeof c && c),
            g = "string" == typeof c ? c : f.slide;
            e || d.data("bs.carousel", e = new b(this, f)),
            "number" == typeof c ? e.to(c) : g ? e[g]() : f.interval && e.pause().cycle()
        })
    },
    a.fn.carousel.Constructor = b,
    a.fn.carousel.noConflict = function () {
        return a.fn.carousel = c,
        this
    },
    a(document).on("click.bs.carousel.data-api", "[data-slide], [data-slide-to]",
    function (b) {
        var c, d = a(this),
        e = a(d.attr("data-target") || (c = d.attr("href")) && c.replace(/.*(?=#[^\s]+$)/, "")),
        f = a.extend({},
        e.data(), d.data()),
        g = d.attr("data-slide-to");
        g && (f.interval = !1),
        e.carousel(f),
        (g = d.attr("data-slide-to")) && e.data("bs.carousel").to(g),
        b.preventDefault()
    }),
    a(window).on("load",
    function () {
        a('[data-ride="carousel"]').each(function () {
            var b = a(this);
            b.carousel(b.data())
        })
    })
}(window.jQuery),
+
function (a) {
    var b = function (c, d) {
        this.$element = a(c),
        this.options = a.extend({},
        b.DEFAULTS, d),
        this.transitioning = null,
        this.options.parent && (this.$parent = a(this.options.parent)),
        this.options.toggle && this.toggle()
    };
    b.DEFAULTS = {
        toggle: !0
    },
    b.prototype.dimension = function () {
        var a = this.$element.hasClass("width");
        return a ? "width" : "height"
    },
    b.prototype.show = function () {
        if (!this.transitioning && !this.$element.hasClass("in")) {
            var b = a.Event("show.bs.collapse");
            if (this.$element.trigger(b), !b.isDefaultPrevented()) {
                var c = this.$parent && this.$parent.find("> .panel > .in");
                if (c && c.length) {
                    var d = c.data("bs.collapse");
                    if (d && d.transitioning) {
                        return
                    }
                    c.collapse("hide"),
                    d || c.data("bs.collapse", null)
                }
                var e = this.dimension();
                this.$element.removeClass("collapse").addClass("collapsing")[e](0),
                this.transitioning = 1;
                var f = function () {
                    this.$element.removeClass("collapsing").addClass("in")[e]("auto"),
                    this.transitioning = 0,
                    this.$element.trigger("shown.bs.collapse")
                };
                if (!a.support.transition) {
                    return f.call(this)
                }
                var g = a.camelCase(["scroll", e].join("-"));
                this.$element.one(a.support.transition.end, a.proxy(f, this)).emulateTransitionEnd(350)[e](this.$element[0][g])
            }
        }
    },
    b.prototype.hide = function () {
        if (!this.transitioning && this.$element.hasClass("in")) {
            var b = a.Event("hide.bs.collapse");
            if (this.$element.trigger(b), !b.isDefaultPrevented()) {
                var c = this.dimension();
                this.$element[c](this.$element[c]())[0].offsetHeight,
                this.$element.addClass("collapsing").removeClass("collapse").removeClass("in"),
                this.transitioning = 1;
                var d = function () {
                    this.transitioning = 0,
                    this.$element.trigger("hidden.bs.collapse").removeClass("collapsing").addClass("collapse")
                };
                return a.support.transition ? (this.$element[c](0).one(a.support.transition.end, a.proxy(d, this)).emulateTransitionEnd(350), void 0) : d.call(this)
            }
        }
    },
    b.prototype.toggle = function () {
        this[this.$element.hasClass("in") ? "hide" : "show"]()
    };
    var c = a.fn.collapse;
    a.fn.collapse = function (c) {
        return this.each(function () {
            var d = a(this),
            e = d.data("bs.collapse"),
            f = a.extend({},
            b.DEFAULTS, d.data(), "object" == typeof c && c);
            e || d.data("bs.collapse", e = new b(this, f)),
            "string" == typeof c && e[c]()
        })
    },
    a.fn.collapse.Constructor = b,
    a.fn.collapse.noConflict = function () {
        return a.fn.collapse = c,
        this
    },
    a(document).on("click.bs.collapse.data-api", "[data-toggle=collapse]",
    function (b) {
        var c, d = a(this),
        e = d.attr("data-target") || b.preventDefault() || (c = d.attr("href")) && c.replace(/.*(?=#[^\s]+$)/, ""),
        f = a(e),
        g = f.data("bs.collapse"),
        h = g ? "toggle" : d.data(),
        i = d.attr("data-parent"),
        j = i && a(i);
        g && g.transitioning || (j && j.find('[data-toggle=collapse][data-parent="' + i + '"]').not(d).addClass("collapsed"), d[f.hasClass("in") ? "addClass" : "removeClass"]("collapsed")),
        f.collapse(h)
    })
}(window.jQuery),
+
function (a) {
    function b() {
        a(d).remove(),
        a(e).each(function (b) {
            var d = c(a(this));
            d.hasClass("open") && (d.trigger(b = a.Event("hide.bs.dropdown")), b.isDefaultPrevented() || d.removeClass("open").trigger("hidden.bs.dropdown"))
        })
    }
    function c(b) {
        var c = b.attr("data-target");
        c || (c = b.attr("href"), c = c && /#/.test(c) && c.replace(/.*(?=#[^\s]*$)/, ""));
        var d = c && a(c);
        return d && d.length ? d : b.parent()
    }
    var d = ".dropdown-backdrop",
    e = "[data-toggle=dropdown]",
    f = function (b) {
        a(b).on("click.bs.dropdown", this.toggle)
    };
    f.prototype.toggle = function (d) {
        var e = a(this);
        if (!e.is(".disabled, :disabled")) {
            var f = c(e),
            g = f.hasClass("open");
            if (b(), !g) {
                if ("ontouchstart" in document.documentElement && !f.closest(".navbar-nav").length && a('<div class="dropdown-backdrop"/>').insertAfter(a(this)).on("click", b), f.trigger(d = a.Event("show.bs.dropdown")), d.isDefaultPrevented()) {
                    return
                }
                f.toggleClass("open").trigger("shown.bs.dropdown"),
                e.focus()
            }
            return !1
        }
    },
    f.prototype.keydown = function (b) {
        if (/(38|40|27)/.test(b.keyCode)) {
            var d = a(this);
            if (b.preventDefault(), b.stopPropagation(), !d.is(".disabled, :disabled")) {
                var f = c(d),
                g = f.hasClass("open");
                if (!g || g && 27 == b.keyCode) {
                    return 27 == b.which && f.find(e).focus(),
                    d.click()
                }
                var h = a("[role=menu] li:not(.divider):visible a", f);
                if (h.length) {
                    var i = h.index(h.filter(":focus"));
                    38 == b.keyCode && i > 0 && i--,
                    40 == b.keyCode && i < h.length - 1 && i++,
                    ~i || (i = 0),
                    h.eq(i).focus()
                }
            }
        }
    };
    var g = a.fn.dropdown;
    a.fn.dropdown = function (b) {
        return this.each(function () {
            var c = a(this),
            d = c.data("dropdown");
            d || c.data("dropdown", d = new f(this)),
            "string" == typeof b && d[b].call(c)
        })
    },
    a.fn.dropdown.Constructor = f,
    a.fn.dropdown.noConflict = function () {
        return a.fn.dropdown = g,
        this
    },
    a(document).on("click.bs.dropdown.data-api", b).on("click.bs.dropdown.data-api", ".dropdown form",
    function (a) {
        a.stopPropagation()
    }).on("click.bs.dropdown.data-api", e, f.prototype.toggle).on("keydown.bs.dropdown.data-api", e + ", [role=menu]", f.prototype.keydown)
}(window.jQuery),
+
function (a) {
    var b = function (b, c) {
        this.options = c,
        this.$element = a(b),
        this.$backdrop = this.isShown = null,
        this.options.remote && this.$element.load(this.options.remote)
    };
    b.DEFAULTS = {
        backdrop: !0,
        keyboard: !0,
        show: !0
    },
    b.prototype.toggle = function (a) {
        return this[this.isShown ? "hide" : "show"](a)
    },
    b.prototype.show = function (b) {
        var c = this,
        d = a.Event("show.bs.modal", {
            relatedTarget: b
        });
        this.$element.trigger(d),
        this.isShown || d.isDefaultPrevented() || (this.isShown = !0, this.escape(), this.$element.on("click.dismiss.modal", '[data-dismiss="modal"]', a.proxy(this.hide, this)), this.backdrop(function () {
            var d = a.support.transition && c.$element.hasClass("fade");
            c.$element.parent().length || c.$element.appendTo(document.body),
            c.$element.show(),
            d && c.$element[0].offsetWidth,
            c.$element.addClass("in").attr("aria-hidden", !1),
            c.enforceFocus();
            var e = a.Event("shown.bs.modal", {
                relatedTarget: b
            });
            d ? c.$element.find(".modal-dialog").one(a.support.transition.end,
            function () {
                c.$element.focus().trigger(e)
            }).emulateTransitionEnd(300) : c.$element.focus().trigger(e)
        }))
    },
    b.prototype.hide = function (b) {
        b && b.preventDefault(),
        b = a.Event("hide.bs.modal"),
        this.$element.trigger(b),
        this.isShown && !b.isDefaultPrevented() && (this.isShown = !1, this.escape(), a(document).off("focusin.bs.modal"), this.$element.removeClass("in").attr("aria-hidden", !0).off("click.dismiss.modal"), a.support.transition && this.$element.hasClass("fade") ? this.$element.one(a.support.transition.end, a.proxy(this.hideModal, this)).emulateTransitionEnd(300) : this.hideModal())
    },
    b.prototype.enforceFocus = function () {
        a(document).off("focusin.bs.modal").on("focusin.bs.modal", a.proxy(function (a) {
            this.$element[0] === a.target || this.$element.has(a.target).length || this.$element.focus()
        },
        this))
    },
    b.prototype.escape = function () {
        this.isShown && this.options.keyboard ? this.$element.on("keyup.dismiss.bs.modal", a.proxy(function (a) {
            27 == a.which && this.hide()
        },
        this)) : this.isShown || this.$element.off("keyup.dismiss.bs.modal")
    },
    b.prototype.hideModal = function () {
        var a = this;
        this.$element.hide(),
        this.backdrop(function () {
            a.removeBackdrop(),
            a.$element.trigger("hidden.bs.modal")
        })
    },
    b.prototype.removeBackdrop = function () {
        this.$backdrop && this.$backdrop.remove(),
        this.$backdrop = null
    },
    b.prototype.backdrop = function (b) {
        var c = this.$element.hasClass("fade") ? "fade" : "";
        if (this.isShown && this.options.backdrop) {
            var d = a.support.transition && c;
            if (this.$backdrop = a('<div class="modal-backdrop ' + c + '" />').appendTo(document.body), this.$element.on("click.dismiss.modal", a.proxy(function (a) {
                a.target === a.currentTarget && ("static" == this.options.backdrop ? this.$element[0].focus.call(this.$element[0]) : this.hide.call(this))
            },
            this)), d && this.$backdrop[0].offsetWidth, this.$backdrop.addClass("in"), !b) {
                return
            }
            d ? this.$backdrop.one(a.support.transition.end, b).emulateTransitionEnd(150) : b()
        } else {
            !this.isShown && this.$backdrop ? (this.$backdrop.removeClass("in"), a.support.transition && this.$element.hasClass("fade") ? this.$backdrop.one(a.support.transition.end, b).emulateTransitionEnd(150) : b()) : b && b()
        }
    };
    var c = a.fn.modal;
    a.fn.modal = function (c, d) {
        return this.each(function () {
            var e = a(this),
            f = e.data("bs.modal"),
            g = a.extend({},
            b.DEFAULTS, e.data(), "object" == typeof c && c);
            f || e.data("bs.modal", f = new b(this, g)),
            "string" == typeof c ? f[c](d) : g.show && f.show(d)
        })
    },
    a.fn.modal.Constructor = b,
    a.fn.modal.noConflict = function () {
        return a.fn.modal = c,
        this
    },
    a(document).on("click.bs.modal.data-api", '[data-toggle="modal"]',
    function (b) {
        var c = a(this),
        d = c.attr("href"),
        e = a(c.attr("data-target") || d && d.replace(/.*(?=#[^\s]+$)/, "")),
        f = e.data("modal") ? "toggle" : a.extend({
            remote: !/#/.test(d) && d
        },
        e.data(), c.data());
        b.preventDefault(),
        e.modal(f, this).one("hide",
        function () {
            c.is(":visible") && c.focus()
        })
    }),
    a(document).on("show.bs.modal", ".modal",
    function () {
        a(document.body).addClass("modal-open")
    }).on("hidden.bs.modal", ".modal",
    function () {
        a(document.body).removeClass("modal-open")
    })
}(window.jQuery),
+
function (a) {
    var b = function (a, b) {
        this.type = this.options = this.enabled = this.timeout = this.hoverState = this.$element = null,
        this.init("tooltip", a, b)
    };
    b.DEFAULTS = {
        animation: !0,
        placement: "top",
        selector: !1,
        template: '<div class="tooltip"><div class="tooltip-arrow"></div><div class="tooltip-inner"></div></div>',
        trigger: "hover focus",
        title: "",
        delay: 0,
        html: !1,
        container: !1
    },
    b.prototype.init = function (b, c, d) {
        this.enabled = !0,
        this.type = b,
        this.$element = a(c),
        this.options = this.getOptions(d);
        for (var e = this.options.trigger.split(" "), f = e.length; f--;) {
            var g = e[f];
            if ("click" == g) {
                this.$element.on("click." + this.type, this.options.selector, a.proxy(this.toggle, this))
            } else {
                if ("manual" != g) {
                    var h = "hover" == g ? "mouseenter" : "focus",
                    i = "hover" == g ? "mouseleave" : "blur";
                    this.$element.on(h + "." + this.type, this.options.selector, a.proxy(this.enter, this)),
                    this.$element.on(i + "." + this.type, this.options.selector, a.proxy(this.leave, this))
                }
            }
        }
        this.options.selector ? this._options = a.extend({},
        this.options, {
            trigger: "manual",
            selector: ""
        }) : this.fixTitle()
    },
    b.prototype.getDefaults = function () {
        return b.DEFAULTS
    },
    b.prototype.getOptions = function (b) {
        return b = a.extend({},
        this.getDefaults(), this.$element.data(), b),
        b.delay && "number" == typeof b.delay && (b.delay = {
            show: b.delay,
            hide: b.delay
        }),
        b
    },
    b.prototype.getDelegateOptions = function () {
        var b = {},
        c = this.getDefaults();
        return this._options && a.each(this._options,
        function (a, d) {
            c[a] != d && (b[a] = d)
        }),
        b
    },
    b.prototype.enter = function (b) {
        var c = b instanceof this.constructor ? b : a(b.currentTarget)[this.type](this.getDelegateOptions()).data("bs." + this.type);
        return clearTimeout(c.timeout),
        c.hoverState = "in",
        c.options.delay && c.options.delay.show ? (c.timeout = setTimeout(function () {
            "in" == c.hoverState && c.show()
        },
        c.options.delay.show), void 0) : c.show()
    },
    b.prototype.leave = function (b) {
        var c = b instanceof this.constructor ? b : a(b.currentTarget)[this.type](this.getDelegateOptions()).data("bs." + this.type);
        return clearTimeout(c.timeout),
        c.hoverState = "out",
        c.options.delay && c.options.delay.hide ? (c.timeout = setTimeout(function () {
            "out" == c.hoverState && c.hide()
        },
        c.options.delay.hide), void 0) : c.hide()
    },
    b.prototype.show = function () {
        var b = a.Event("show.bs." + this.type);
        if (this.hasContent() && this.enabled) {
            if (this.$element.trigger(b), b.isDefaultPrevented()) {
                return
            }
            var c = this.tip();
            this.setContent(),
            this.options.animation && c.addClass("fade");
            var d = "function" == typeof this.options.placement ? this.options.placement.call(this, c[0], this.$element[0]) : this.options.placement,
            e = /\s?auto?\s?/i,
            f = e.test(d);
            f && (d = d.replace(e, "") || "top"),
            c.detach().css({
                top: 0,
                left: 0,
                display: "block"
            }).addClass(d),
            this.options.container ? c.appendTo(this.options.container) : c.insertAfter(this.$element);
            var g = this.getPosition(),
            h = c[0].offsetWidth,
            i = c[0].offsetHeight;
            if (f) {
                var j = this.$element.parent(),
                k = d,
                l = document.documentElement.scrollTop || document.body.scrollTop,
                m = "body" == this.options.container ? window.innerWidth : j.outerWidth(),
                n = "body" == this.options.container ? window.innerHeight : j.outerHeight(),
                o = "body" == this.options.container ? 0 : j.offset().left;
                d = "bottom" == d && g.top + g.height + i - l > n ? "top" : "top" == d && g.top - l - i < 0 ? "bottom" : "right" == d && g.right + h > m ? "left" : "left" == d && g.left - h < o ? "right" : d,
                c.removeClass(k).addClass(d)
            }
            var p = this.getCalculatedOffset(d, g, h, i);
            this.applyPlacement(p, d),
            this.$element.trigger("shown.bs." + this.type)
        }
    },
    b.prototype.applyPlacement = function (a, b) {
        var c, d = this.tip(),
        e = d[0].offsetWidth,
        f = d[0].offsetHeight,
        g = parseInt(d.css("margin-top"), 10),
        h = parseInt(d.css("margin-left"), 10);
        isNaN(g) && (g = 0),
        isNaN(h) && (h = 0),
        a.top = a.top + g,
        a.left = a.left + h,
        d.offset(a).addClass("in");
        var i = d[0].offsetWidth,
        j = d[0].offsetHeight;
        if ("top" == b && j != f && (c = !0, a.top = a.top + f - j), /bottom|top/.test(b)) {
            var k = 0;
            a.left < 0 && (k = -2 * a.left, a.left = 0, d.offset(a), i = d[0].offsetWidth, j = d[0].offsetHeight),
            this.replaceArrow(k - e + i, i, "left")
        } else {
            this.replaceArrow(j - f, j, "top")
        }
        c && d.offset(a)
    },
    b.prototype.replaceArrow = function (a, b, c) {
        this.arrow().css(c, a ? 50 * (1 - a / b) + "%" : "")
    },
    b.prototype.setContent = function () {
        var a = this.tip(),
        b = this.getTitle();
        a.find(".tooltip-inner")[this.options.html ? "html" : "text"](b),
        a.removeClass("fade in top bottom left right")
    },
    b.prototype.hide = function () {
        function b() {
            "in" != c.hoverState && d.detach()
        }
        var c = this,
        d = this.tip(),
        e = a.Event("hide.bs." + this.type);
        return this.$element.trigger(e),
        e.isDefaultPrevented() ? void 0 : (d.removeClass("in"), a.support.transition && this.$tip.hasClass("fade") ? d.one(a.support.transition.end, b).emulateTransitionEnd(150) : b(), this.$element.trigger("hidden.bs." + this.type), this)
    },
    b.prototype.fixTitle = function () {
        var a = this.$element; (a.attr("title") || "string" != typeof a.attr("data-original-title")) && a.attr("data-original-title", a.attr("title") || "").attr("title", "")
    },
    b.prototype.hasContent = function () {
        return this.getTitle()
    },
    b.prototype.getPosition = function () {
        var b = this.$element[0];
        return a.extend({},
        "function" == typeof b.getBoundingClientRect ? b.getBoundingClientRect() : {
            width: b.offsetWidth,
            height: b.offsetHeight
        },
        this.$element.offset())
    },
    b.prototype.getCalculatedOffset = function (a, b, c, d) {
        return "bottom" == a ? {
            top: b.top + b.height,
            left: b.left + b.width / 2 - c / 2
        } : "top" == a ? {
            top: b.top - d,
            left: b.left + b.width / 2 - c / 2
        } : "left" == a ? {
            top: b.top + b.height / 2 - d / 2,
            left: b.left - c
        } : {
            top: b.top + b.height / 2 - d / 2,
            left: b.left + b.width
        }
    },
    b.prototype.getTitle = function () {
        var a, b = this.$element,
        c = this.options;
        return a = b.attr("data-original-title") || ("function" == typeof c.title ? c.title.call(b[0]) : c.title)
    },
    b.prototype.tip = function () {
        return this.$tip = this.$tip || a(this.options.template)
    },
    b.prototype.arrow = function () {
        return this.$arrow = this.$arrow || this.tip().find(".tooltip-arrow")
    },
    b.prototype.validate = function () {
        this.$element[0].parentNode || (this.hide(), this.$element = null, this.options = null)
    },
    b.prototype.enable = function () {
        this.enabled = !0
    },
    b.prototype.disable = function () {
        this.enabled = !1
    },
    b.prototype.toggleEnabled = function () {
        this.enabled = !this.enabled
    },
    b.prototype.toggle = function (b) {
        var c = b ? a(b.currentTarget)[this.type](this.getDelegateOptions()).data("bs." + this.type) : this;
        c.tip().hasClass("in") ? c.leave(c) : c.enter(c)
    },
    b.prototype.destroy = function () {
        this.hide().$element.off("." + this.type).removeData("bs." + this.type)
    };
    var c = a.fn.tooltip;
    a.fn.tooltip = function (c) {
        return this.each(function () {
            var d = a(this),
            e = d.data("bs.tooltip"),
            f = "object" == typeof c && c;
            e || d.data("bs.tooltip", e = new b(this, f)),
            "string" == typeof c && e[c]()
        })
    },
    a.fn.tooltip.Constructor = b,
    a.fn.tooltip.noConflict = function () {
        return a.fn.tooltip = c,
        this
    }
}(window.jQuery),
+
function (a) {
    var b = function (a, b) {
        this.init("popover", a, b)
    };
    if (!a.fn.tooltip) {
        throw new Error("Popover requires tooltip.js")
    }
    b.DEFAULTS = a.extend({},
    a.fn.tooltip.Constructor.DEFAULTS, {
        placement: "right",
        trigger: "click",
        content: "",
        template: '<div class="popover"><div class="arrow"></div><h3 class="popover-title"></h3><div class="popover-content"></div></div>'
    }),
    b.prototype = a.extend({},
    a.fn.tooltip.Constructor.prototype),
    b.prototype.constructor = b,
    b.prototype.getDefaults = function () {
        return b.DEFAULTS
    },
    b.prototype.setContent = function () {
        var a = this.tip(),
        b = this.getTitle(),
        c = this.getContent();
        a.find(".popover-title")[this.options.html ? "html" : "text"](b),
        a.find(".popover-content")[this.options.html ? "html" : "text"](c),
        a.removeClass("fade top bottom left right in"),
        a.find(".popover-title").html() || a.find(".popover-title").hide()
    },
    b.prototype.hasContent = function () {
        return this.getTitle() || this.getContent()
    },
    b.prototype.getContent = function () {
        var a = this.$element,
        b = this.options;
        return a.attr("data-content") || ("function" == typeof b.content ? b.content.call(a[0]) : b.content)
    },
    b.prototype.arrow = function () {
        return this.$arrow = this.$arrow || this.tip().find(".arrow")
    },
    b.prototype.tip = function () {
        return this.$tip || (this.$tip = a(this.options.template)),
        this.$tip
    };
    var c = a.fn.popover;
    a.fn.popover = function (c) {
        return this.each(function () {
            var d = a(this),
            e = d.data("bs.popover"),
            f = "object" == typeof c && c;
            e || d.data("bs.popover", e = new b(this, f)),
            "string" == typeof c && e[c]()
        })
    },
    a.fn.popover.Constructor = b,
    a.fn.popover.noConflict = function () {
        return a.fn.popover = c,
        this
    }
}(window.jQuery),
+
function (a) {
    function b(c, d) {
        var e, f = a.proxy(this.process, this);
        this.$element = a(c).is("body") ? a(window) : a(c),
        this.$body = a("body"),
        this.$scrollElement = this.$element.on("scroll.bs.scroll-spy.data-api", f),
        this.options = a.extend({},
        b.DEFAULTS, d),
        this.selector = (this.options.target || (e = a(c).attr("href")) && e.replace(/.*(?=#[^\s]+$)/, "") || "") + " .nav li > a",
        this.offsets = a([]),
        this.targets = a([]),
        this.activeTarget = null,
        this.refresh(),
        this.process()
    }
    b.DEFAULTS = {
        offset: 10
    },
    b.prototype.refresh = function () {
        var b = this.$element[0] == window ? "offset" : "position";
        this.offsets = a([]),
        this.targets = a([]);
        var c = this;
        this.$body.find(this.selector).map(function () {
            var d = a(this),
            e = d.data("target") || d.attr("href"),
            f = /^#\w/.test(e) && a(e);
            return f && f.length && [[f[b]().top + (!a.isWindow(c.$scrollElement.get(0)) && c.$scrollElement.scrollTop()), e]] || null
        }).sort(function (a, b) {
            return a[0] - b[0]
        }).each(function () {
            c.offsets.push(this[0]),
            c.targets.push(this[1])
        })
    },
    b.prototype.process = function () {
        var a, b = this.$scrollElement.scrollTop() + this.options.offset,
        c = this.$scrollElement[0].scrollHeight || this.$body[0].scrollHeight,
        d = c - this.$scrollElement.height(),
        e = this.offsets,
        f = this.targets,
        g = this.activeTarget;
        if (b >= d) {
            return g != (a = f.last()[0]) && this.activate(a)
        }
        for (a = e.length; a--;) {
            g != f[a] && b >= e[a] && (!e[a + 1] || b <= e[a + 1]) && this.activate(f[a])
        }
    },
    b.prototype.activate = function (b) {
        this.activeTarget = b,
        a(this.selector).parents(".active").removeClass("active");
        var c = this.selector + '[data-target="' + b + '"],' + this.selector + '[href="' + b + '"]',
        d = a(c).parents("li").addClass("active");
        d.parent(".dropdown-menu").length && (d = d.closest("li.dropdown").addClass("active")),
        d.trigger("activate")
    };
    var c = a.fn.scrollspy;
    a.fn.scrollspy = function (c) {
        return this.each(function () {
            var d = a(this),
            e = d.data("bs.scrollspy"),
            f = "object" == typeof c && c;
            e || d.data("bs.scrollspy", e = new b(this, f)),
            "string" == typeof c && e[c]()
        })
    },
    a.fn.scrollspy.Constructor = b,
    a.fn.scrollspy.noConflict = function () {
        return a.fn.scrollspy = c,
        this
    },
    a(window).on("load",
    function () {
        a('[data-spy="scroll"]').each(function () {
            var b = a(this);
            b.scrollspy(b.data())
        })
    })
}(window.jQuery),
+
function (a) {
    var b = function (b) {
        this.element = a(b)
    };
    b.prototype.show = function () {
        var b = this.element,
        c = b.closest("ul:not(.dropdown-menu)"),
        d = b.attr("data-target");
        if (d || (d = b.attr("href"), d = d && d.replace(/.*(?=#[^\s]*$)/, "")), !b.parent("li").hasClass("active")) {
            var e = c.find(".active:last a")[0],
            f = a.Event("show.bs.tab", {
                relatedTarget: e
            });
            if (b.trigger(f), !f.isDefaultPrevented()) {
                var g = a(d);
                this.activate(b.parent("li"), c),
                this.activate(g, g.parent(),
                function () {
                    b.trigger({
                        type: "shown.bs.tab",
                        relatedTarget: e
                    })
                })
            }
        }
    },
    b.prototype.activate = function (b, c, d) {
        function e() {
            f.removeClass("active").find("> .dropdown-menu > .active").removeClass("active"),
            b.addClass("active"),
            g ? (b[0].offsetWidth, b.addClass("in")) : b.removeClass("fade"),
            b.parent(".dropdown-menu") && b.closest("li.dropdown").addClass("active"),
            d && d()
        }
        var f = c.find("> .active"),
        g = d && a.support.transition && f.hasClass("fade");
        g ? f.one(a.support.transition.end, e).emulateTransitionEnd(150) : e(),
        f.removeClass("in")
    };
    var c = a.fn.tab;
    a.fn.tab = function (c) {
        return this.each(function () {
            var d = a(this),
            e = d.data("bs.tab");
            e || d.data("bs.tab", e = new b(this)),
            "string" == typeof c && e[c]()
        })
    },
    a.fn.tab.Constructor = b,
    a.fn.tab.noConflict = function () {
        return a.fn.tab = c,
        this
    },
    a(document).on("click.bs.tab.data-api", '[data-toggle="tab"], [data-toggle="pill"]',
    function (b) {
        b.preventDefault(),
        a(this).tab("show")
    })
}(window.jQuery),
+
function (a) {
    var b = function (c, d) {
        this.options = a.extend({},
        b.DEFAULTS, d),
        this.$window = a(window).on("scroll.bs.affix.data-api", a.proxy(this.checkPosition, this)).on("click.bs.affix.data-api", a.proxy(this.checkPositionWithEventLoop, this)),
        this.$element = a(c),
        this.affixed = this.unpin = null,
        this.checkPosition()
    };
    b.RESET = "affix affix-top affix-bottom",
    b.DEFAULTS = {
        offset: 0
    },
    b.prototype.checkPositionWithEventLoop = function () {
        setTimeout(a.proxy(this.checkPosition, this), 1)
    },
    b.prototype.checkPosition = function () {
        if (this.$element.is(":visible")) {
            var c = a(document).height(),
            d = this.$window.scrollTop(),
            e = this.$element.offset(),
            f = this.options.offset,
            g = f.top,
            h = f.bottom;
            "object" != typeof f && (h = g = f),
            "function" == typeof g && (g = f.top()),
            "function" == typeof h && (h = f.bottom());
            var i = null != this.unpin && d + this.unpin <= e.top ? !1 : null != h && e.top + this.$element.height() >= c - h ? "bottom" : null != g && g >= d ? "top" : !1;
            this.affixed !== i && (this.unpin && this.$element.css("top", ""), this.affixed = i, this.unpin = "bottom" == i ? e.top - d : null, this.$element.removeClass(b.RESET).addClass("affix" + (i ? "-" + i : "")), "bottom" == i && this.$element.offset({
                top: document.body.offsetHeight - h - this.$element.height()
            }))
        }
    };
    var c = a.fn.affix;
    a.fn.affix = function (c) {
        return this.each(function () {
            var d = a(this),
            e = d.data("bs.affix"),
            f = "object" == typeof c && c;
            e || d.data("bs.affix", e = new b(this, f)),
            "string" == typeof c && e[c]()
        })
    },
    a.fn.affix.Constructor = b,
    a.fn.affix.noConflict = function () {
        return a.fn.affix = c,
        this
    },
    a(window).on("load",
    function () {
        a('[data-spy="affix"]').each(function () {
            var b = a(this),
            c = b.data();
            c.offset = c.offset || {},
            c.offsetBottom && (c.offset.bottom = c.offsetBottom),
            c.offsetTop && (c.offset.top = c.offsetTop),
            b.affix(c)
        })
    })
}(window.jQuery);
if (!("ace" in window)) {
    window.ace = {}
}
ace.config = {
    cookie_expiry: 604800,
    storage_method: 2
};
ace.settings = {
    is: function (b, a) {
        return (ace.data.get("settings", b + "-" + a) == 1)
    },
    exists: function (b, a) {
        return (ace.data.get("settings", b + "-" + a) !== null)
    },
    set: function (b, a) {
        ace.data.set("settings", b + "-" + a, 1)
    },
    unset: function (b, a) {
        ace.data.set("settings", b + "-" + a, -1)
    },
    remove: function (b, a) {
        ace.data.remove("settings", b + "-" + a)
    },
    navbar_fixed: function (a) {
        a = a || false;
        if (!a && ace.settings.is("sidebar", "fixed")) {
            ace.settings.sidebar_fixed(false)
        }
        var b = document.getElementById("navbar");
        if (a) {
            if (!ace.hasClass(b, "navbar-fixed-top")) {
                ace.addClass(b, "navbar-fixed-top")
            }
            if (!ace.hasClass(document.body, "navbar-fixed")) {
                ace.addClass(document.body, "navbar-fixed")
            }
            ace.settings.set("navbar", "fixed")
        } else {
            ace.removeClass(b, "navbar-fixed-top");
            ace.removeClass(document.body, "navbar-fixed");
            ace.settings.unset("navbar", "fixed")
        }
        document.getElementById("ace-settings-navbar").checked = a
    },
    breadcrumbs_fixed: function (a) {
        a = a || false;
        if (a && !ace.settings.is("sidebar", "fixed")) {
            ace.settings.sidebar_fixed(true)
        }
        var b = document.getElementById("breadcrumbs");
        if (a) {
            if (!ace.hasClass(b, "breadcrumbs-fixed")) {
                ace.addClass(b, "breadcrumbs-fixed")
            }
            if (!ace.hasClass(document.body, "breadcrumbs-fixed")) {
                ace.addClass(document.body, "breadcrumbs-fixed")
            }
            ace.settings.set("breadcrumbs", "fixed")
        } else {
            ace.removeClass(b, "breadcrumbs-fixed");
            ace.removeClass(document.body, "breadcrumbs-fixed");
            ace.settings.unset("breadcrumbs", "fixed")
        }
        document.getElementById("ace-settings-breadcrumbs").checked = a
    },
    sidebar_fixed: function (a) {
        a = a || false;
        if (!a && ace.settings.is("breadcrumbs", "fixed")) {
            ace.settings.breadcrumbs_fixed(false)
        }
        if (a && !ace.settings.is("navbar", "fixed")) {
            ace.settings.navbar_fixed(true)
        }
        var b = document.getElementById("sidebar");
        if (a) {
            if (!ace.hasClass(b, "sidebar-fixed")) {
                ace.addClass(b, "sidebar-fixed")
            }
            ace.settings.set("sidebar", "fixed")
        } else {
            ace.removeClass(b, "sidebar-fixed");
            ace.settings.unset("sidebar", "fixed")
        }
        document.getElementById("ace-settings-sidebar").checked = a
    },
    main_container_fixed: function (a) {
        a = a || false;
        var c = document.getElementById("main-container");
        var b = document.getElementById("navbar-container");
        if (a) {
            if (!ace.hasClass(c, "container")) {
                ace.addClass(c, "container")
            }
            if (!ace.hasClass(b, "container")) {
                ace.addClass(b, "container")
            }
            ace.settings.set("main-container", "fixed")
        } else {
            ace.removeClass(c, "container");
            ace.removeClass(b, "container");
            ace.settings.unset("main-container", "fixed")
        }
        document.getElementById("ace-settings-add-container").checked = a;
        if (navigator.userAgent.match(/webkit/i)) {
            var d = document.getElementById("sidebar");
            ace.toggleClass(d, "menu-min");
            setTimeout(function () {
                ace.toggleClass(d, "menu-min")
            },
            0)
        }
    },
    sidebar_collapsed: function (c) {
        c = c || false;
        var e = document.getElementById("sidebar");
        var d = document.getElementById("sidebar-collapse").querySelector('[class*="icon-"]');
        var b = d.getAttribute("data-icon1");
        var a = d.getAttribute("data-icon2");
        if (c) {
            ace.addClass(e, "menu-min");
            ace.removeClass(d, b);
            ace.addClass(d, a);
            ace.settings.set("sidebar", "collapsed")
        } else {
            ace.removeClass(e, "menu-min");
            ace.removeClass(d, a);
            ace.addClass(d, b);
            ace.settings.unset("sidebar", "collapsed")
        }
    },
};
ace.settings.check = function (c, e) {
    if (!ace.settings.exists(c, e)) {
        return
    }
    var a = ace.settings.is(c, e);
    var b = {
        "navbar-fixed": "navbar-fixed-top",
        "sidebar-fixed": "sidebar-fixed",
        "breadcrumbs-fixed": "breadcrumbs-fixed",
        "sidebar-collapsed": "menu-min",
        "main-container-fixed": "container"
    };
    var d = document.getElementById(c);
    if (a != ace.hasClass(d, b[c + "-" + e])) {
        ace.settings[c.replace("-", "_") + "_" + e](a)
    }
};
ace.data_storage = function (e, c) {
    var b = "ace.";
    var d = null;
    var a = 0;
    if ((e == 1 || e === c) && "localStorage" in window && window.localStorage !== null) {
        d = ace.storage;
        a = 1
    } else {
        if (d == null && (e == 2 || e === c) && "cookie" in document && document.cookie !== null) {
            d = ace.cookie;
            a = 2
        }
    }
    this.set = function (h, g, i, k) {
        if (!d) {
            return
        }
        if (i === k) {
            i = g;
            g = h;
            if (i == null) {
                d.remove(b + g)
            } else {
                if (a == 1) {
                    d.set(b + g, i)
                } else {
                    if (a == 2) {
                        d.set(b + g, i, ace.config.cookie_expiry)
                    }
                }
            }
        } else {
            if (a == 1) {
                if (i == null) {
                    d.remove(b + h + "." + g)
                } else {
                    d.set(b + h + "." + g, i)
                }
            } else {
                if (a == 2) {
                    var j = d.get(b + h);
                    var f = j ? JSON.parse(j) : {};
                    if (i == null) {
                        delete f[g];
                        if (ace.sizeof(f) == 0) {
                            d.remove(b + h);
                            return
                        }
                    } else {
                        f[g] = i
                    }
                    d.set(b + h, JSON.stringify(f), ace.config.cookie_expiry)
                }
            }
        }
    };
    this.get = function (h, g, j) {
        if (!d) {
            return null
        }
        if (g === j) {
            g = h;
            return d.get(b + g)
        } else {
            if (a == 1) {
                return d.get(b + h + "." + g)
            } else {
                if (a == 2) {
                    var i = d.get(b + h);
                    var f = i ? JSON.parse(i) : {};
                    return g in f ? f[g] : null
                }
            }
        }
    };
    this.remove = function (g, f, h) {
        if (!d) {
            return
        }
        if (f === h) {
            f = g;
            this.set(f, null)
        } else {
            this.set(g, f, null)
        }
    }
};
ace.cookie = {
    get: function (c) {
        var d = document.cookie,
        g, f = c + "=",
        a;
        if (!d) {
            return
        }
        a = d.indexOf("; " + f);
        if (a == -1) {
            a = d.indexOf(f);
            if (a != 0) {
                return null
            }
        } else {
            a += 2
        }
        g = d.indexOf(";", a);
        if (g == -1) {
            g = d.length
        }
        return decodeURIComponent(d.substring(a + f.length, g))
    },
    set: function (b, e, a, g, c, f) {
        var h = new Date();
        if (typeof (a) == "object" && a.toGMTString) {
            a = a.toGMTString()
        } else {
            if (parseInt(a, 10)) {
                h.setTime(h.getTime() + (parseInt(a, 10) * 1000));
                a = h.toGMTString()
            } else {
                a = ""
            }
        }
        document.cookie = b + "=" + encodeURIComponent(e) + ((a) ? "; expires=" + a : "") + ((g) ? "; path=" + g : "") + ((c) ? "; domain=" + c : "") + ((f) ? "; secure" : "")
    },
    remove: function (a, b) {
        this.set(a, "", -1000, b)
    }
};
ace.storage = {
    get: function (a) {
        return window.localStorage.getItem(a)
    },
    set: function (a, b) {
        window.localStorage.setItem(a, b)
    },
    remove: function (a) {
        window.localStorage.removeItem(a)
    }
};
ace.sizeof = function (c) {
    var b = 0;
    for (var a in c) {
        if (c.hasOwnProperty(a)) {
            b++
        }
    }
    return b
};
ace.hasClass = function (b, a) {
    return (" " + b.className + " ").indexOf(" " + a + " ") > -1
};
ace.addClass = function (c, b) {
    if (!ace.hasClass(c, b)) {
        var a = c.className;
        c.className = a + (a.length ? " " : "") + b
    }
};
ace.removeClass = function (b, a) {
    ace.replaceClass(b, a)
};
ace.replaceClass = function (c, b, d) {
    var a = new RegExp(("(^|\\s)" + b + "(\\s|$)"), "i");
    c.className = c.className.replace(a,
    function (e, g, f) {
        return d ? (g + d + f) : " "
    }).replace(/^\s+|\s+$/g, "")
};
ace.toggleClass = function (b, a) {
    if (ace.hasClass(b, a)) {
        ace.removeClass(b, a)
    } else {
        ace.addClass(b, a)
    }
};
ace.data = new ace.data_storage(ace.config.storage_method);
if (!("ace" in window)) {
    window.ace = {}
}
jQuery(function (a) {
    window.ace.click_event = a.fn.tap ? "tap" : "click"
});
jQuery(function (a) {
    ace.handle_side_menu(jQuery);
    ace.enable_search_ahead(jQuery);
    ace.general_things(jQuery);
    ace.widget_boxes(jQuery);
    ace.widget_reload_handler(jQuery)
});
ace.handle_side_menu = function (a) {
    a("#menu-toggler").on(ace.click_event,
    function () {
        a("#sidebar").toggleClass("display");
        a(this).toggleClass("display");
        return false
    });
    var c = a("#sidebar").hasClass("menu-min");
    a("#sidebar-collapse").on(ace.click_event,
    function () {
        c = a("#sidebar").hasClass("menu-min");
        ace.settings.sidebar_collapsed(!c)
    });
    var b = navigator.userAgent.match(/OS (5|6|7)(_\d)+ like Mac OS X/i);
    a(".nav-list").on(ace.click_event,
    function (h) {
        var g = a(h.target).closest("a");
        if (!g || g.length == 0) {
            return
        }
        c = a("#sidebar").hasClass("menu-min");
        if (!g.hasClass("dropdown-toggle")) {
            if (c && ace.click_event == "tap" && g.get(0).parentNode.parentNode == this) {
                var i = g.find(".menu-text").get(0);
                if (h.target != i && !a.contains(i, h.target)) {
                    return false
                }
            }
            if (b) {
                document.location = g.attr("href");
                return false
            }
            return
        }
        var f = g.next().get(0);
        if (!a(f).is(":visible")) {
            var d = a(f.parentNode).closest("ul");
            if (c && d.hasClass("nav-list")) {
                return
            }
            d.find("> .open > .submenu").each(function () {
                if (this != f && !a(this.parentNode).hasClass("active")) {
                    a(this).slideUp(200).parent().removeClass("open")
                }
            })
        } else { }
        if (c && a(f.parentNode.parentNode).hasClass("nav-list")) {
            return false
        }
        a(f).slideToggle(200).parent().toggleClass("open");
        return false
    })
};
ace.general_things = function (a) {
    a('.ace-nav [class*="icon-animated-"]').closest("a").on("click",
    function () {
        var d = a(this).find('[class*="icon-animated-"]').eq(0);
        var c = d.attr("class").match(/icon\-animated\-([\d\w]+)/);
        d.removeClass(c[0]);
        a(this).off("click")
    });
    a(".nav-list .badge[title],.nav-list .label[title]").tooltip({
        placement: "right"
    });
    a("#ace-settings-btn").on(ace.click_event,
    function () {
        a(this).toggleClass("open");
        a("#ace-settings-box").toggleClass("open")
    });
    a("#ace-settings-navbar").on("click",
    function () {
        ace.settings.navbar_fixed(this.checked)
    }).each(function () {
        this.checked = ace.settings.is("navbar", "fixed")
    });
    a("#ace-settings-sidebar").on("click",
    function () {
        ace.settings.sidebar_fixed(this.checked)
    }).each(function () {
        this.checked = ace.settings.is("sidebar", "fixed")
    });
    a("#ace-settings-breadcrumbs").on("click",
    function () {
        ace.settings.breadcrumbs_fixed(this.checked)
    }).each(function () {
        this.checked = ace.settings.is("breadcrumbs", "fixed")
    });
    a("#ace-settings-add-container").on("click",
    function () {
        ace.settings.main_container_fixed(this.checked)
    }).each(function () {
        this.checked = ace.settings.is("main-container", "fixed")
    });
    a("#ace-settings-rtl").removeAttr("checked").on("click",
    function () {
        ace.switch_direction(jQuery)
    });
    a("#btn-scroll-up").on(ace.click_event,
    function () {
        var c = Math.min(400, Math.max(100, parseInt(a("html").scrollTop() / 3)));
        a("html,body").animate({
            scrollTop: 0
        },
        c);
        return false
    });
    try {
        a("#skin-colorpicker").ace_colorpicker()
    } catch (b) { }
    a("#skin-colorpicker").on("change",
    function () {
        var d = a(this).find("option:selected").data("skin");
        var c = a(document.body);
        c.removeClass("skin-1 skin-2 skin-3");
        if (d != "default") {
            c.addClass(d)
        }
        if (d == "skin-1") {
            a(".ace-nav > li.grey").addClass("dark")
        } else {
            a(".ace-nav > li.grey").removeClass("dark")
        }
        if (d == "skin-2") {
            a(".ace-nav > li").addClass("no-border margin-1");
            a(".ace-nav > li:not(:last-child)").addClass("light-pink").find('> a > [class*="icon-"]').addClass("pink").end().eq(0).find(".badge").addClass("badge-warning")
        } else {
            a(".ace-nav > li").removeClass("no-border margin-1");
            a(".ace-nav > li:not(:last-child)").removeClass("light-pink").find('> a > [class*="icon-"]').removeClass("pink").end().eq(0).find(".badge").removeClass("badge-warning")
        }
        if (d == "skin-3") {
            a(".ace-nav > li.grey").addClass("red").find(".badge").addClass("badge-yellow")
        } else {
            a(".ace-nav > li.grey").removeClass("red").find(".badge").removeClass("badge-yellow")
        }
    })
};
ace.widget_boxes = function (a) {
    a(document).on("hide.bs.collapse show.bs.collapse",
    function (c) {
        var b = c.target.getAttribute("id");
        a('[href*="#' + b + '"]').find('[class*="icon-"]').each(function () {
            var e = a(this);
            var d;
            var f = null;
            var g = null;
            if ((f = e.attr("data-icon-show"))) {
                g = e.attr("data-icon-hide")
            } else {
                if (d = e.attr("class").match(/icon\-(.*)\-(up|down)/)) {
                    f = "icon-" + d[1] + "-down";
                    g = "icon-" + d[1] + "-up"
                }
            }
            if (f) {
                if (c.type == "show") {
                    e.removeClass(f).addClass(g)
                } else {
                    e.removeClass(g).addClass(f)
                }
                return false
            }
        })
    });
    a(document).on("click.ace.widget", "[data-action]",
    function (o) {
        o.preventDefault();
        var n = a(this);
        var p = n.data("action");
        var b = n.closest(".widget-box");
        if (b.hasClass("ui-sortable-helper")) {
            return
        }
        if (p == "collapse") {
            var j = b.hasClass("collapsed") ? "show" : "hide";
            var f = j == "show" ? "shown" : "hidden";
            var c;
            b.trigger(c = a.Event(j + ".ace.widget"));
            if (c.isDefaultPrevented()) {
                return
            }
            var g = b.find(".widget-body");
            var m = n.find("[class*=icon-]").eq(0);
            var h = m.attr("class").match(/icon\-(.*)\-(up|down)/);
            var d = "icon-" + h[1] + "-down";
            var i = "icon-" + h[1] + "-up";
            var l = g.find(".widget-body-inner");
            if (l.length == 0) {
                g = g.wrapInner('<div class="widget-body-inner"></div>').find(":first-child").eq(0)
            } else {
                g = l.eq(0)
            }
            var e = 300;
            var k = 200;
            if (j == "show") {
                if (m) {
                    m.addClass(i).removeClass(d)
                }
                b.removeClass("collapsed");
                g.slideUp(0,
                function () {
                    g.slideDown(e,
                    function () {
                        b.trigger(c = a.Event(f + ".ace.widget"))
                    })
                })
            } else {
                if (m) {
                    m.addClass(d).removeClass(i)
                }
                g.slideUp(k,
                function () {
                    b.addClass("collapsed");
                    b.trigger(c = a.Event(f + ".ace.widget"))
                })
            }
        } else {
            if (p == "close") {
                var c;
                b.trigger(c = a.Event("close.ace.widget"));
                if (c.isDefaultPrevented()) {
                    return
                }
                var r = parseInt(n.data("close-speed")) || 300;
                b.hide(r,
                function () {
                    b.trigger(c = a.Event("closed.ace.widget"));
                    b.remove()
                })
            } else {
                if (p == "reload") {
                    var c;
                    b.trigger(c = a.Event("reload.ace.widget"));
                    if (c.isDefaultPrevented()) {
                        return
                    }
                    n.blur();
                    var q = false;
                    if (b.css("position") == "static") {
                        q = true;
                        b.addClass("position-relative")
                    }
                    b.append('<div class="widget-box-overlay"><i class="icon-spinner icon-spin icon-2x white"></i></div>');
                    b.one("reloaded.ace.widget",
                    function () {
                        b.find(".widget-box-overlay").remove();
                        if (q) {
                            b.removeClass("position-relative")
                        }
                    })
                } else {
                    if (p == "settings") {
                        var c = a.Event("settings.ace.widget");
                        b.trigger(c)
                    }
                }
            }
        }
    })
};
ace.widget_reload_handler = function (a) {
    a(document).on("reload.ace.widget", ".widget-box",
    function (b) {
        var c = a(this);
        setTimeout(function () {
            c.trigger("reloaded.ace.widget")
        },
        parseInt(Math.random() * 1000 + 1000))
    })
};
ace.enable_search_ahead = function (a) {
    ace.variable_US_STATES = ["Alabama", "Alaska", "Arizona", "Arkansas", "California", "Colorado", "Connecticut", "Delaware", "Florida", "Georgia", "Hawaii", "Idaho", "Illinois", "Indiana", "Iowa", "Kansas", "Kentucky", "Louisiana", "Maine", "Maryland", "Massachusetts", "Michigan", "Minnesota", "Mississippi", "Missouri", "Montana", "Nebraska", "Nevada", "New Hampshire", "New Jersey", "New Mexico", "New York", "North Dakota", "North Carolina", "Ohio", "Oklahoma", "Oregon", "Pennsylvania", "Rhode Island", "South Carolina", "South Dakota", "Tennessee", "Texas", "Utah", "Vermont", "Virginia", "Washington", "West Virginia", "Wisconsin", "Wyoming"];
    try {
        a("#nav-search-input").typeahead({
            source: ace.variable_US_STATES,
            updater: function (c) {
                a("#nav-search-input").focus();
                return c
            }
        })
    } catch (b) { }
};
ace.switch_direction = function (d) {
    var c = d(document.body);
    c.toggleClass("rtl").find(".dropdown-menu:not(.datepicker-dropdown,.colorpicker)").toggleClass("pull-right").end().find(".pull-right:not(.dropdown-menu,blockquote,.profile-skills .pull-right)").removeClass("pull-right").addClass("tmp-rtl-pull-right").end().find(".pull-left:not(.dropdown-submenu,.profile-skills .pull-left)").removeClass("pull-left").addClass("pull-right").end().find(".tmp-rtl-pull-right").removeClass("tmp-rtl-pull-right").addClass("pull-left").end().find(".chosen-container").toggleClass("chosen-rtl").end();
    function a(h, g) {
        c.find("." + h).removeClass(h).addClass("tmp-rtl-" + h).end().find("." + g).removeClass(g).addClass(h).end().find(".tmp-rtl-" + h).removeClass("tmp-rtl-" + h).addClass(g)
    }
    function b(h, g, i) {
        i.each(function () {
            var k = d(this);
            var j = k.css(g);
            k.css(g, k.css(h));
            k.css(h, j)
        })
    }
    a("align-left", "align-right");
    a("no-padding-left", "no-padding-right");
    a("arrowed", "arrowed-right");
    a("arrowed-in", "arrowed-in-right");
    a("messagebar-item-left", "messagebar-item-right");
    var e = d("#piechart-placeholder");
    if (e.size() > 0) {
        var f = d(document.body).hasClass("rtl") ? "nw" : "ne";
        e.data("draw").call(e.get(0), e, e.data("chart"), f)
    }
};
if (!("ace" in window)) {
    window.ace = {}
}
jQuery(function () {
    window.ace.click_event = $.fn.tap ? "tap" : "click"
}); (function (e, c) {
    var d = "multiple" in document.createElement("INPUT");
    var j = "FileList" in window;
    var b = "FileReader" in window;
    var f = function (l, m) {
        var k = this;
        this.settings = e.extend({},
        e.fn.ace_file_input.defaults, m);
        this.$element = e(l);
        this.element = l;
        this.disabled = false;
        this.can_reset = true;
        this.$element.on("change.ace_inner_call",
        function (o, n) {
            if (n === true) {
                return
            }
            return a.call(k)
        });
        this.$element.wrap('<div class="ace-file-input" />');
        this.apply_settings()
    };
    f.error = {
        FILE_LOAD_FAILED: 1,
        IMAGE_LOAD_FAILED: 2,
        THUMBNAIL_FAILED: 3
    };
    f.prototype.apply_settings = function () {
        var l = this;
        var k = !!this.settings.icon_remove;
        this.multi = this.$element.attr("multiple") && d;
        this.well_style = this.settings.style == "well";
        if (this.well_style) {
            this.$element.parent().addClass("ace-file-multiple")
        } else {
            this.$element.parent().removeClass("ace-file-multiple")
        }
        this.$element.parent().find(":not(input[type=file])").remove();
        this.$element.after('<label class="file-label" data-title="' + this.settings.btn_choose + '"><span class="file-name" data-title="' + this.settings.no_file + '">' + (this.settings.no_icon ? '<i class="' + this.settings.no_icon + '"></i>' : "") + "</span></label>" + (k ? '<a class="remove" href="#"><i class="' + this.settings.icon_remove + '"></i></a>' : ""));
        this.$label = this.$element.next();
        this.$label.on("click",
        function () {
            if (!this.disabled && !l.element.disabled && !l.$element.attr("readonly")) {
                l.$element.click()
            }
        });
        if (k) {
            this.$label.next("a").on(ace.click_event,
            function () {
                if (!l.can_reset) {
                    return false
                }
                var m = true;
                if (l.settings.before_remove) {
                    m = l.settings.before_remove.call(l.element)
                }
                if (!m) {
                    return false
                }
                return l.reset_input()
            })
        }
        if (this.settings.droppable && j) {
            g.call(this)
        }
    };
    f.prototype.show_file_list = function (k) {
        var n = typeof k === "undefined" ? this.$element.data("ace_input_files") : k;
        if (!n || n.length == 0) {
            return
        }
        if (this.well_style) {
            this.$label.find(".file-name").remove();
            if (!this.settings.btn_change) {
                this.$label.addClass("hide-placeholder")
            }
        }
        this.$label.attr("data-title", this.settings.btn_change).addClass("selected");
        for (var p = 0; p < n.length; p++) {
            var l = typeof n[p] === "string" ? n[p] : e.trim(n[p].name);
            var q = l.lastIndexOf("\\") + 1;
            if (q == 0) {
                q = l.lastIndexOf("/") + 1
            }
            l = l.substr(q);
            var m = "icon-file";
            if ((/\.(jpe?g|png|gif|svg|bmp|tiff?)$/i).test(l)) {
                m = "icon-picture"
            } else {
                if ((/\.(mpe?g|flv|mov|avi|swf|mp4|mkv|webm|wmv|3gp)$/i).test(l)) {
                    m = "icon-film"
                } else {
                    if ((/\.(mp3|ogg|wav|wma|amr|aac)$/i).test(l)) {
                        m = "icon-music"
                    }
                }
            }
            if (!this.well_style) {
                this.$label.find(".file-name").attr({
                    "data-title": l
                }).find('[class*="icon-"]').attr("class", m)
            } else {
                this.$label.append('<span class="file-name" data-title="' + l + '"><i class="' + m + '"></i></span>');
                var r = e.trim(n[p].type);
                var o = b && this.settings.thumbnail && ((r.length > 0 && r.match("image")) || (r.length == 0 && m == "icon-picture"));
                if (o) {
                    var s = this;
                    e.when(i.call(this, n[p])).fail(function (t) {
                        if (s.settings.preview_error) {
                            s.settings.preview_error.call(s, l, t.code)
                        }
                    })
                }
            }
        }
        return true
    };
    f.prototype.reset_input = function () {
        this.$label.attr({
            "data-title": this.settings.btn_choose,
            "class": "file-label"
        }).find(".file-name:first").attr({
            "data-title": this.settings.no_file,
            "class": "file-name"
        }).find('[class*="icon-"]').attr("class", this.settings.no_icon).prev("img").remove();
        if (!this.settings.no_icon) {
            this.$label.find('[class*="icon-"]').remove()
        }
        this.$label.find(".file-name").not(":first").remove();
        if (this.$element.data("ace_input_files")) {
            this.$element.removeData("ace_input_files");
            this.$element.removeData("ace_input_method")
        }
        this.reset_input_field();
        return false
    };
    f.prototype.reset_input_field = function () {
        this.$element.wrap("<form>").closest("form").get(0).reset();
        this.$element.unwrap()
    };
    f.prototype.enable_reset = function (k) {
        this.can_reset = k
    };
    f.prototype.disable = function () {
        this.disabled = true;
        this.$element.attr("disabled", "disabled").addClass("disabled")
    };
    f.prototype.enable = function () {
        this.disabled = false;
        this.$element.removeAttr("disabled").removeClass("disabled")
    };
    f.prototype.files = function () {
        return e(this).data("ace_input_files") || null
    };
    f.prototype.method = function () {
        return e(this).data("ace_input_method") || ""
    };
    f.prototype.update_settings = function (k) {
        this.settings = e.extend({},
        this.settings, k);
        this.apply_settings()
    };
    var g = function () {
        var l = this;
        var k = this.element.parentNode;
        e(k).on("dragenter",
        function (m) {
            m.preventDefault();
            m.stopPropagation()
        }).on("dragover",
        function (m) {
            m.preventDefault();
            m.stopPropagation()
        }).on("drop",
        function (q) {
            q.preventDefault();
            q.stopPropagation();
            var p = q.originalEvent.dataTransfer;
            var o = p.files;
            if (!l.multi && o.length > 1) {
                var n = [];
                n.push(o[0]);
                o = n
            }
            var m = true;
            if (l.settings.before_change) {
                m = l.settings.before_change.call(l.element, o, true)
            }
            if (!m || m.length == 0) {
                return false
            }
            if (m instanceof Array || (j && m instanceof FileList)) {
                o = m
            }
            l.$element.data("ace_input_files", o);
            l.$element.data("ace_input_method", "drop");
            l.show_file_list(o);
            l.$element.triggerHandler("change", [true]);
            return true
        })
    };
    var a = function () {
        var l = true;
        if (this.settings.before_change) {
            l = this.settings.before_change.call(this.element, this.element.files || [this.element.value], false)
        }
        if (!l || l.length == 0) {
            if (!this.$element.data("ace_input_files")) {
                this.reset_input_field()
            }
            return false
        }
        var m = !j ? null : ((l instanceof Array || l instanceof FileList) ? l : this.element.files);
        this.$element.data("ace_input_method", "select");
        if (m && m.length > 0) {
            this.$element.data("ace_input_files", m)
        } else {
            var k = e.trim(this.element.value);
            if (k && k.length > 0) {
                m = [];
                m.push(k);
                this.$element.data("ace_input_files", m)
            }
        }
        if (!m || m.length == 0) {
            return false
        }
        this.show_file_list(m);
        return true
    };
    var i = function (o) {
        var n = this;
        var l = n.$label.find(".file-name:last");
        var m = new e.Deferred;
        var k = new FileReader();
        k.onload = function (q) {
            l.prepend("<img class='middle' style='display:none;' />");
            var p = l.find("img:last").get(0);
            e(p).one("load",
            function () {
                var t = 50;
                if (n.settings.thumbnail == "large") {
                    t = 150
                } else {
                    if (n.settings.thumbnail == "fit") {
                        t = l.width()
                    }
                }
                l.addClass(t > 50 ? "large" : "");
                var s = h(p, t, o.type);
                if (s == null) {
                    e(this).remove();
                    m.reject({
                        code: f.error.THUMBNAIL_FAILED
                    });
                    return
                }
                var r = s.w,
                u = s.h;
                if (n.settings.thumbnail == "small") {
                    r = u = t
                }
                e(p).css({
                    "background-image": "url(" + s.src + ")",
                    width: r,
                    height: u
                }).data("thumb", s.src).attr({
                    src: "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVQImWNgYGBgAAAABQABh6FO1AAAAABJRU5ErkJggg=="
                }).show();
                m.resolve()
            }).one("error",
            function () {
                l.find("img").remove();
                m.reject({
                    code: f.error.IMAGE_LOAD_FAILED
                })
            });
            p.src = q.target.result
        };
        k.onerror = function (p) {
            m.reject({
                code: f.error.FILE_LOAD_FAILED
            })
        };
        k.readAsDataURL(o);
        return m.promise()
    };
    var h = function (n, s, q) {
        var r = n.width,
        o = n.height;
        if (r > s || o > s) {
            if (r > o) {
                o = parseInt(s / r * o);
                r = s
            } else {
                r = parseInt(s / o * r);
                o = s
            }
        }
        var m;
        try {
            var l = document.createElement("canvas");
            l.width = r;
            l.height = o;
            var k = l.getContext("2d");
            k.drawImage(n, 0, 0, n.width, n.height, 0, 0, r, o);
            m = l.toDataURL()
        } catch (p) {
            m = null
        }
        if (!(/^data\:image\/(png|jpe?g|gif);base64,[0-9A-Za-z\+\/\=]+$/.test(m))) {
            m = null
        }
        if (!m) {
            return null
        }
        return {
            src: m,
            w: r,
            h: o
        }
    };
    e.fn.ace_file_input = function (m, n) {
        var l;
        var k = this.each(function () {
            var q = e(this);
            var p = q.data("ace_file_input");
            var o = typeof m === "object" && m;
            if (!p) {
                q.data("ace_file_input", (p = new f(this, o)))
            }
            if (typeof m === "string") {
                l = p[m](n)
            }
        });
        return (l === c) ? k : l
    };
    e.fn.ace_file_input.defaults = {
        style: false,
        no_file: "No File ...",
        no_icon: "icon-upload-alt",
        btn_choose: "Choose",
        btn_change: "Change",
        icon_remove: "icon-remove",
        droppable: false,
        thumbnail: false,
        before_change: null,
        before_remove: null,
        preview_error: null
    }
})(window.jQuery); (function (a, b) {
    a.fn.ace_spinner = function (c) {
        this.each(function () {
            var f = c.icon_up || "icon-chevron-up";
            var j = c.icon_down || "icon-chevron-down";
            var h = c.on_sides || false;
            var e = c.btn_up_class || "";
            var g = c.btn_down_class || "";
            var d = c.max || 999;
            d = ("" + d).length;
            a(this).addClass("spinner-input form-control").wrap('<div class="ace-spinner">');
            var k = a(this).closest(".ace-spinner").spinner(c).wrapInner("<div class='input-group'></div>");
            if (h) {
                a(this).before('<div class="spinner-buttons input-group-btn">							<button type="button" class="btn spinner-down btn-xs ' + g + '">								<i class="' + j + '"></i>							</button>						</div>').after('<div class="spinner-buttons input-group-btn">							<button type="button" class="btn spinner-up btn-xs ' + e + '">								<i class="' + f + '"></i>							</button>						</div>');
                k.addClass("touch-spinner");
                k.css("width", (d * 20 + 40) + "px")
            } else {
                a(this).after('<div class="spinner-buttons input-group-btn">							<button type="button" class="btn spinner-up btn-xs ' + e + '">								<i class="' + f + '"></i>							</button>							<button type="button" class="btn spinner-down btn-xs ' + g + '">								<i class="' + j + '"></i>							</button>						</div>');
                if ("ontouchend" in document || c.touch_spinner) {
                    k.addClass("touch-spinner");
                    k.css("width", (d * 20 + 40) + "px")
                } else {
                    a(this).next().addClass("btn-group-vertical");
                    k.css("width", (d * 20 + 10) + "px")
                }
            }
            a(this).on("mousewheel DOMMouseScroll",
            function (l) {
                var m = l.originalEvent.detail < 0 || l.originalEvent.wheelDelta > 0 ? 1 : -1;
                k.spinner("step", m > 0);
                k.spinner("triggerChangedEvent");
                return false
            });
            var i = a(this);
            k.on("changed",
            function () {
                i.trigger("change")
            })
        });
        return this
    }
})(window.jQuery); (function (a, b) {
    a.fn.ace_wizard = function (c) {
        this.each(function () {
            var e = a(this);
            e.wizard();
            var d = e.siblings(".wizard-actions").eq(0);
            var f = e.data("wizard");
            f.$prevBtn.remove();
            f.$nextBtn.remove();
            f.$prevBtn = d.find(".btn-prev").eq(0).on(ace.click_event,
            function () {
                e.wizard("previous")
            }).attr("disabled", "disabled");
            f.$nextBtn = d.find(".btn-next").eq(0).on(ace.click_event,
            function () {
                e.wizard("next")
            }).removeAttr("disabled");
            f.nextText = f.$nextBtn.text()
        });
        return this
    }
})(window.jQuery); (function (a, b) {
    a.fn.ace_colorpicker = function (c) {
        var d = a.extend({
            pull_right: false,
            caret: true
        },
        c);
        this.each(function () {
            var g = a(this);
            var e = "";
            var f = "";
            a(this).hide().find("option").each(function () {
                var h = "colorpick-btn";
                if (this.selected) {
                    h += " selected";
                    f = this.value
                }
                e += '<li><a class="' + h + '" href="#" style="background-color:' + this.value + ';" data-color="' + this.value + '"></a></li>'
            }).end().on("change.ace_inner_call",
            function () {
                a(this).next().find(".btn-colorpicker").css("background-color", this.value)
            }).after('<div class="dropdown dropdown-colorpicker"><a data-toggle="dropdown" class="dropdown-toggle" href="#"><span class="btn-colorpicker" style="background-color:' + f + '"></span></a><ul class="dropdown-menu' + (d.caret ? " dropdown-caret" : "") + (d.pull_right ? " pull-right" : "") + '">' + e + "</ul></div>").next().find(".dropdown-menu").on(ace.click_event,
            function (j) {
                var h = a(j.target);
                if (!h.is(".colorpick-btn")) {
                    return false
                }
                h.closest("ul").find(".selected").removeClass("selected");
                h.addClass("selected");
                var i = h.data("color");
                g.val(i).change();
                j.preventDefault();
                return true
            })
        });
        return this
    }
})(window.jQuery); (function (a, b) {
    a.fn.ace_tree = function (d) {
        var c = {
            "open-icon": "icon-folder-open",
            "close-icon": "icon-folder-close",
            selectable: true,
            "selected-icon": "icon-ok",
            "unselected-icon": "tree-dot"
        };
        c = a.extend({},
        c, d);
        this.each(function () {
            var e = a(this);
            e.html('<div class = "tree-folder" style="display:none;">				<div class="tree-folder-header">					<i class="' + c["close-icon"] + '"></i>					<div class="tree-folder-name"></div>				</div>				<div class="tree-folder-content"></div>				<div class="tree-loader" style="display:none"></div>			</div>			<div class="tree-item" style="display:none;">				' + (c["unselected-icon"] == null ? "" : '<i class="' + c["unselected-icon"] + '"></i>') + '				<div class="tree-item-name"></div>			</div>');
            e.addClass(c.selectable == true ? "tree-selectable" : "tree-unselectable");
            e.tree(c)
        });
        return this
    }
})(window.jQuery); (function (a, b) {
    a.fn.ace_wysiwyg = function (c, h) {
        var d = a.extend({
            speech_button: true,
            wysiwyg: {}
        },
        c);
        var e = ["#ac725e", "#d06b64", "#f83a22", "#fa573c", "#ff7537", "#ffad46", "#42d692", "#16a765", "#7bd148", "#b3dc6c", "#fbe983", "#fad165", "#92e1c0", "#9fe1e7", "#9fc6e7", "#4986e7", "#9a9cff", "#b99aff", "#c2c2c2", "#cabdbf", "#cca6ac", "#f691b2", "#cd74e6", "#a47ae2", "#444444"];
        var g = {
            font: {
                values: ["Arial", "Courier", "Comic Sans MS", "Helvetica", "Open Sans", "Tahoma", "Verdana"],
                icon: "icon-font",
                title: "Font"
            },
            fontSize: {
                values: {
                    5: "Huge",
                    3: "Normal",
                    1: "Small"
                },
                icon: "icon-text-height",
                title: "Font Size"
            },
            bold: {
                icon: "icon-bold",
                title: "Bold (Ctrl/Cmd+B)"
            },
            italic: {
                icon: "icon-italic",
                title: "Italic (Ctrl/Cmd+I)"
            },
            strikethrough: {
                icon: "icon-strikethrough",
                title: "Strikethrough"
            },
            underline: {
                icon: "icon-underline",
                title: "Underline"
            },
            insertunorderedlist: {
                icon: "icon-list-ul",
                title: "Bullet list"
            },
            insertorderedlist: {
                icon: "icon-list-ol",
                title: "Number list"
            },
            outdent: {
                icon: "icon-indent-left",
                title: "Reduce indent (Shift+Tab)"
            },
            indent: {
                icon: "icon-indent-right",
                title: "Indent (Tab)"
            },
            justifyleft: {
                icon: "icon-align-left",
                title: "Align Left (Ctrl/Cmd+L)"
            },
            justifycenter: {
                icon: "icon-align-center",
                title: "Center (Ctrl/Cmd+E)"
            },
            justifyright: {
                icon: "icon-align-right",
                title: "Align Right (Ctrl/Cmd+R)"
            },
            justifyfull: {
                icon: "icon-align-justify",
                title: "Justify (Ctrl/Cmd+J)"
            },
            createLink: {
                icon: "icon-link",
                title: "Hyperlink",
                button_text: "Add",
                placeholder: "URL",
                button_class: "btn-primary"
            },
            unlink: {
                icon: "icon-unlink",
                title: "Remove Hyperlink"
            },
            insertImage: {
                icon: "icon-picture",
                title: "Insert picture",
                button_text: '<i class="icon-file"></i> Choose Image &hellip;',
                placeholder: "Image URL",
                button_insert: "Insert",
                button_class: "btn-success",
                button_insert_class: "btn-primary",
                choose_file: true
            },
            foreColor: {
                values: e,
                title: "Change Color"
            },
            backColor: {
                values: e,
                title: "Change Background Color"
            },
            undo: {
                icon: "icon-undo",
                title: "Undo (Ctrl/Cmd+Z)"
            },
            redo: {
                icon: "icon-repeat",
                title: "Redo (Ctrl/Cmd+Y)"
            },
            viewSource: {
                icon: "icon-code",
                title: "View Source"
            }
        };
        var f = d.toolbar || ["font", null, "fontSize", null, "bold", "italic", "strikethrough", "underline", null, "insertunorderedlist", "insertorderedlist", "outdent", "indent", null, "justifyleft", "justifycenter", "justifyright", "justifyfull", null, "createLink", "unlink", null, "insertImage", null, "foreColor", null, "undo", "redo", null, "viewSource"];
        this.each(function () {
            var r = ' <div class="wysiwyg-toolbar btn-toolbar center"> <div class="btn-group"> ';
            for (var n in f) {
                if (f.hasOwnProperty(n)) {
                    var p = f[n];
                    if (p === null) {
                        r += ' </div> <div class="btn-group"> ';
                        continue
                    }
                    if (typeof p == "string" && p in g) {
                        p = g[p];
                        p.name = f[n]
                    } else {
                        if (typeof p == "object" && p.name in g) {
                            p = a.extend(g[p.name], p)
                        } else {
                            continue
                        }
                    }
                    var q = "className" in p ? p.className : "";
                    switch (p.name) {
                        case "font":
                            r += ' <a class="btn btn-sm ' + q + ' dropdown-toggle" data-toggle="dropdown" title="' + p.title + '"><i class="' + p.icon + '"></i><i class="icon-angle-down icon-on-right"></i></a> ';
                            r += ' <ul class="dropdown-menu dropdown-light">';
                            for (var j in p.values) {
                                if (p.values.hasOwnProperty(j)) {
                                    r += ' <li><a data-edit="fontName ' + p.values[j] + '" style="font-family:\'' + p.values[j] + "'\">" + p.values[j] + "</a></li> "
                                }
                            }
                            r += " </ul>";
                            break;
                        case "fontSize":
                            r += ' <a class="btn btn-sm ' + q + ' dropdown-toggle" data-toggle="dropdown" title="' + p.title + '"><i class="' + p.icon + '"></i>&nbsp;<i class="icon-angle-down icon-on-right"></i></a> ';
                            r += ' <ul class="dropdown-menu dropdown-light"> ';
                            for (var t in p.values) {
                                if (p.values.hasOwnProperty(t)) {
                                    r += ' <li><a data-edit="fontSize ' + t + '"><font size="' + t + '">' + p.values[t] + "</font></a></li> "
                                }
                            }
                            r += " </ul> ";
                            break;
                        case "createLink":
                            r += ' <div class="inline position-relative"> <a class="btn btn-sm ' + q + ' dropdown-toggle" data-toggle="dropdown" title="' + p.title + '"><i class="' + p.icon + '"></i></a> ';
                            r += ' <div class="dropdown-menu dropdown-caret pull-right">							<div class="input-group">								<input class="form-control" placeholder="' + p.placeholder + '" type="text" data-edit="' + p.name + '" />								<span class="input-group-btn">									<button class="btn btn-sm ' + p.button_class + '" type="button">' + p.button_text + "</button>								</span>							</div>						</div> </div>";
                            break;
                        case "insertImage":
                            r += ' <div class="inline position-relative"> <a class="btn btn-sm ' + q + ' dropdown-toggle" data-toggle="dropdown" title="' + p.title + '"><i class="' + p.icon + '"></i></a> ';
                            r += ' <div class="dropdown-menu dropdown-caret pull-right">							<div class="input-group">								<input class="form-control" placeholder="' + p.placeholder + '" type="text" data-edit="' + p.name + '" />								<span class="input-group-btn">									<button class="btn btn-sm ' + p.button_insert_class + '" type="button">' + p.button_insert + "</button>								</span>							</div>";
                            if (p.choose_file && "FileReader" in window) {
                                r += '<div class="space-2"></div>							 <div class="center">								<button class="btn btn-sm ' + p.button_class + ' wysiwyg-choose-file" type="button">' + p.button_text + '</button>								<input type="file" data-edit="' + p.name + '" />							  </div>'
                            }
                            r += " </div> </div>";
                            break;
                        case "foreColor":
                        case "backColor":
                            r += ' <select class="hide wysiwyg_colorpicker" title="' + p.title + '"> ';
                            for (var m in p.values) {
                                r += ' <option value="' + p.values[m] + '">' + p.values[m] + "</option> "
                            }
                            r += " </select> ";
                            r += ' <input style="display:none;" disabled class="hide" type="text" data-edit="' + p.name + '" /> ';
                            break;
                        case "viewSource":
                            r += ' <a class="btn btn-sm ' + q + '" data-view="source" title="' + p.title + '"><i class="' + p.icon + '"></i></a> ';
                            break;
                        default:
                            r += ' <a class="btn btn-sm ' + q + '" data-edit="' + p.name + '" title="' + p.title + '"><i class="' + p.icon + '"></i></a> ';
                            break
                    }
                }
            }
            r += " </div> </div> ";
            if (d.toolbar_place) {
                r = d.toolbar_place.call(this, r)
            } else {
                r = a(this).before(r).prev()
            }
            r.find("a[title]").tooltip({
                animation: false,
                container: "body"
            });
            r.find(".dropdown-menu input:not([type=file])").on(ace.click_event,
            function () {
                return false
            }).on("change",
            function () {
                a(this).closest(".dropdown-menu").siblings(".dropdown-toggle").dropdown("toggle")
            }).on("keydown",
            function (u) {
                if (u.which == 27) {
                    this.value = "";
                    a(this).change()
                }
            });
            r.find("input[type=file]").prev().on(ace.click_event,
            function (u) {
                a(this).next().click()
            });
            r.find(".wysiwyg_colorpicker").each(function () {
                a(this).ace_colorpicker({
                    pull_right: true
                }).change(function () {
                    a(this).nextAll("input").eq(0).val(this.value).change()
                }).next().find(".btn-colorpicker").tooltip({
                    title: this.title,
                    animation: false,
                    container: "body"
                })
            });
            var k;
            if (d.speech_button && "onwebkitspeechchange" in (k = document.createElement("input"))) {
                var i = a(this).offset();
                r.append(k);
                a(k).attr({
                    type: "text",
                    "data-edit": "inserttext",
                    "x-webkit-speech": ""
                }).addClass("wysiwyg-speech-input").css({
                    position: "absolute"
                }).offset({
                    top: i.top,
                    left: i.left + a(this).innerWidth() - 35
                })
            } else {
                k = null
            }
            var s = a(this);
            var l = false;
            r.find("a[data-view=source]").on("click",
            function (v) {
                v.preventDefault();
                if (!l) {
                    a("<textarea />").css({
                        width: s.outerWidth(),
                        height: s.outerHeight()
                    }).val(s.html()).insertAfter(s);
                    s.hide();
                    a(this).addClass("active")
                } else {
                    var u = s.next();
                    s.html(u.val()).show();
                    u.remove();
                    a(this).removeClass("active")
                }
                l = !l
            });
            var o = a.extend({},
            {
                activeToolbarClass: "active",
                toolbarSelector: r
            },
            d.wysiwyg || {});
            a(this).wysiwyg(o)
        });
        return this
    }
})(window.jQuery);