//注册命名空间
namespace = new Object();
namespace.register = function (str) {
    var namespaceArr = str.split('.');
    var fobj = null;
    for (var i = 0; i < namespaceArr.length; i++) {
        var obj = namespaceArr[i];
        if (i == 0) {
            if (typeof window[obj] == "undefined") {
                window[obj] = new Object();
            }
            fobj = window[obj];
        }
        else if (i != 0) {
            if (typeof fobj[obj] == "undefined") {
                fobj[obj] = new Object();
            }
            fobj = fobj[obj];
        }
    }
}

//写cookies
function setCookie(name, value) {
    var Days = 30;
    var exp = new Date();
    exp.setTime(exp.getTime() + Days * 24 * 60 * 60 * 1000);
    document.cookie = name + "=" + escape(value) + ";expires=" + exp.toGMTString();
}

function getCookie(name) {
    var arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");
    if (arr = document.cookie.match(reg))
        return unescape(arr[2]);
    else
        return null;
}

function delCookie(name) {
    var exp = new Date();
    exp.setTime(exp.getTime() - 1);
    var cval = getCookie(name);
    if (cval != null)
        document.cookie = name + "=" + cval + ";expires=" + exp.toGMTString();
}


function openwindow(query, w) {
    if (w == 0) { w = 1200; }
    var l = (screen.width - w) / 2;
    var h = screen.height - 300;
    winprops = 'resizable=0, height=' + h + ',width=' + w + ',top=120px,left=' + l + 'px';
    // if (scroll) winprops += ',scrollbars=1';
    // return showModalDialog(query, window, 'dialogWidth:' + w + 'px;dialogHeight:' + h + 'px;dialogLeft:' + l + 'px;dialogTop:100px;center:yes;help:yes;resizable:yes;status:yes');
    window.open(query, "_blank", winprops);

}


function request(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return r[2]; return null;
}


$.format = function (source, params) {
    if (arguments.length == 1)
        return function () {
            var args = $.makeArray(arguments);
            args.unshift(source);
            return $.validator.format.apply(this, args);
        };
    if (arguments.length > 2 && params.constructor != Array) {
        params = $.makeArray(arguments).slice(1);
    }
    if (params.constructor != Array) {
        params = [params];
    }
    $.each(params, function (i, n) {
        source = source.replace(new RegExp("\\{" + i + "\\}", "g"), (typeof n == 'undefined' || n == null) ? '' : n);
    });
    return source;
}

Array.prototype.indexOf = function (val) {
    for (var i = 0; i < this.length; i++) {
        if (this[i] == val) return i;
    }
    return -1;
};


Array.prototype.remove = function (val) {
    var index = this.indexOf(val);
    if (index > -1) {
        this.splice(index, 1);
    }
};

Array.prototype.removeobj = function (key, val) {
    for (var i = 0; i < this.length; i++) {
        var obj = this[i];
        if (obj[key] == val) {
            this.splice(i, 1);
        }
    }
};

Array.prototype.insert = function (index, item) {
    this.splice(index, 0, item);
};

Array.prototype.getobj = function (key, val) {
    for (var i = 0; i < this.length; i++) {
        var obj = this[i];
        if (obj[key] == val) {
            return obj;
        }
    }
};

// 对Date的扩展，将 Date 转化为指定格式的String
// 月(M)、日(d)、小时(h)、分(m)、秒(s)、季度(q) 可以用 1-2 个占位符， 
// 年(y)可以用 1-4 个占位符，毫秒(S)只能用 1 个占位符(是 1-3 位的数字) 
// 例子： 
// (new Date()).Format("yyyy-MM-dd hh:mm:ss.S") ==> 2006-07-02 08:09:04.423 
// (new Date()).Format("yyyy-M-d h:m:s.S")      ==> 2006-7-2 8:9:4.18 
Date.prototype.Format = function (fmt) { //author: meizz 
    var o = {
        "M+": this.getMonth() + 1, //月份 
        "d+": this.getDate(), //日 
        "H+": this.getHours(), //小时 
        "m+": this.getMinutes(), //分 
        "s+": this.getSeconds(), //秒 
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度 
        "S": this.getMilliseconds() //毫秒 
    };
    if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}

function addDate(date, days) {
    var d = new Date(date);
    d.setDate(d.getDate() + days);
    var m = d.getMonth() + 1;
    return d.getFullYear() + '-' + m + '-' + d.getDate();

}

function addMoth(d, m) {
    var ds = d.split('-');
    d = new Date(ds[0], ds[1] - 1, ds[2])
    d.setMonth(d.getMonth() + parseInt(m));
    return d.Format("yyyy-MM-dd");
}

//时间相差天数
function TimeDiffDay(d1, d2) {
    if (typeof d1 == "string") {
        d1 = Date.parse(d1)
    }
    if (typeof d2 == "string") {
        d2 = Date.parse(d2)
    }
    var d = d2 - d1  //时间差的毫秒数
    return Math.floor(d / (24 * 3600 * 1000));
}



//只能输入整数
function onlynumber(e) {
    var maxlength = parseInt($(e).attr("maxlength"));
    $(e).val($(e).val().replace(/[^\d]/g, ''));
    if ($(e).val().length >= maxlength) {
        $(e).val($(e).val().toString().substring(0, maxlength));
    }
}


$(function () {
    $("body").delegate(".numberbox", "keyup", function (e) { onlynumber(this); });
    $("body").delegate(".numberbox", "blur", function (e) { onlynumber(this); });
});


function Refresh() {
    // var isfresh
}

var refreshtask = {
    add: function (key) {
        $.cookie(key, "true", { path: '/' });
    },
    run: function (key, callback) {
        if (key != null) {
            var isrefresh = $.cookie(key);
            if (isrefresh == "true") {
                $.cookie(key, null, { path: '/' });
                if (typeof callback == 'function') {
                    callback();
                }
            }
        }
    }
}
