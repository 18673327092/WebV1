﻿jQuery.fn.floatdiv = function (a) { var b = false; if ($.browser.msie && $.browser.version == "6.0") { $("html").css("overflow-x", "auto").css("overflow-y", "hidden"); b = true; } $("body").css({ margin: "0px", border: "0px", height: "100%", overflow: "auto" }); return this.each(function () { var f; if (a == undefined || a.constructor == String) { switch (a) { case ("rightbottom"): f = { right: "0px", bottom: "0px" }; break; case ("leftbottom"): f = { left: "0px", bottom: "0px" }; break; case ("lefttop"): f = { left: "0px", top: "0px" }; break; case ("righttop"): f = { right: "0px", top: "0px" }; break; case ("middle"): var c = 0; var e = 0; var d, g; if (self.innerHeight) { d = self.innerWidth; g = self.innerHeight; } else { if (document.documentElement && document.documentElement.clientHeight) { d = document.documentElement.clientWidth; g = document.documentElement.clientHeight; } else { if (document.body) { d = document.body.clientWidth; g = document.body.clientHeight; } } } c = d / 2 - $(this).width() / 2; e = g / 2 - $(this).height() / 2; f = { left: c + "px", top: e + "px" }; break; default: f = { right: "0px", bottom: "0px" }; break; } } else { f = a; } $(this).css("z-index", "9999").css(f).css("position", "fixed"); if (b) { if (f.right != undefined) { if ($(this).css("right") == null || $(this).css("right") == "") { $(this).css("right", "18px"); } } $(this).css("position", "absolute"); } }); };