﻿var tempData = {}; var noData = false; (function ($) { $.fn.autoComplete = function (config) { var timeid; var lastValue; var lastValueDate; var isScanBar; var options; var c = $(this); var d; var t; var headerPropertys = new Array(); var defaults = { width: c.width(), maxheight: 264, top: 0, header: null, url: "", type: "post", async: true, autoLength: 0, businame: "bill", getValue: function (value) { }, selected: function (text) { }, canceled: function () { } }; options = $.extend(defaults, config); var p = c.position(); d = $('<div class="autoCompleteBox"></div>'); d.attr("id", "autoComplete_Group_" + Math.random()); c.after(d); d.css({ left: p.left, top: p.top + c.height() + options.top, width: options.width, "max-height": options.maxheight, "margin-top": 10, "z-index": 1000 }); t = $('<table cellspacing="0" cellpadding="5"></table>'); var htdiv = $("<div></div>"); d.append(htdiv); d.append(t); c.unbind("keydown").bind("keydown", keydown_process); c.unbind("keyup").bind("keyup", keyup_process); c.unbind("blur").bind("blur", blur_process); d.unbind("focus").bind("focus", focus_div); d.unbind("mouseout").bind("mouseout", mouseout_div); function blur_process() { options.canceled(); timeid = setTimeout(function () { }, 200); } function mouseout_div() { } function focus_div() { clearTimeout(timeid); c.focus(); } function keydown_process(e) { if (d.is(":hidden")) { return; } switch (e.keyCode) { case 38: e.preventDefault(); var p = t.find(".nowRow").prev(); if (p.length > 0) { d.setScroll(options.maxheight, p, d); d.setHeaderTop(htdiv); t.find(".nowRow").removeClass("nowRow"); t.find("td").css({ background: "#FFFFFF", "border-bottom": "soild 1px #d7d7d7" }); $(p).addClass("nowRow"); $(p).find("td").css({ background: "#ecf4ff", "border-bottom": "soild 1px #bdd1eb" }); } else { p = t.find("tr:last"); if (p.length > 0) { d.setScroll(options.maxheight, p, d); d.setHeaderTop(htdiv); t.find(".nowRow").removeClass("nowRow"); t.find("td").css({ background: "#FFFFFF", "border-bottom": "soild 1px #d7d7d7" }); $(p).addClass("nowRow"); $(p).find("td").css({ background: "#ecf4ff", "border-bottom": "soild 1px #bdd1eb" }); } } return false; break; case 40: e.preventDefault(); var n = t.find(".nowRow").next(); if (n.length > 0) { d.setScroll(options.maxheight, n, d); d.setHeaderTop(htdiv); t.find(".nowRow").removeClass("nowRow"); t.find("td").css({ background: "#FFFFFF", "border-bottom": "soild 1px #d7d7d7" }); $(n).addClass("nowRow"); $(n).find("td").css({ background: "#ecf4ff", "border-bottom": "soild 1px #bdd1eb" }); } else { n = t.find("tr:first"); if (n.length > 0) { d.setScroll(options.maxheight, n, d); d.setHeaderTop(htdiv); t.find(".nowRow").removeClass("nowRow"); t.find("td").css({ background: "#FFFFFF", "border-bottom": "soild 1px #d7d7d7" }); $(n).addClass("nowRow"); $(n).find("td").css({ background: "#ecf4ff", "border-bottom": "soild 1px #bdd1eb" }); } } return false; break; case 13: e.preventDefault(); var n = t.find(".nowRow"); if (n.length > 0) { options.selected(StringToJson(n.attr("rowValue"))); lastValue = ""; setTimeout(function () { d.hide(); }, 10); } break; case 39: e.preventDefault(); var n = t.find(".nowRow"); if (n.length > 0) { options.selected(StringToJson(n.attr("rowValue"))); lastValue = ""; d.hide(); } break; } } function keyup_process(e) { if (e.keyCode == 13 || e.keyCode == 38 || (!d.is(":hidden") && e.keyCode == 40) || e.keyCode == 37 || e.keyCode == 39) { return true; } if (!noData && e.keyCode == 13) { setTimeout(function () { e.preventDefault(); var n = t.find(".nowRow"); if (n.length > 0) { options.selected(StringToJson(n.attr("rowValue"))); options.lastValue = ""; d.hide(); } }, 200); } $(c).find(".choseGoods").addClass("processing"); var lvd = options.lastValueDate; var curDate = new Date(); if (lvd != undefined && (curDate.getTime() - lvd.getTime()) < 30) { options.isScanBar = true; } else { options.isScanBar = false; } options.lastValueDate = new Date(); setTimeout(function () { if (c.val() == "") { options.lastValue = ""; d.hide(); return; } if (options.lastValue != c.val()) { if (c.val() == "" || c.val().length > options.autoLength) { var datatemp = tempData[escape(requestByUrl("branchid", options.url) + requestByUrl("WarehouseId", options.url) + options.businame + c.val())]; if (datatemp != undefined) { processdata(datatemp); } else { getData(c, function (data) { processdata(data); }); } options.lastValue = c.val(); if (t.find("thead>th").length > 0) { } } else { options.lastValue = ""; d.hide(); } } }, 200); } function processdata(data) { if ($(c).parent().find(".spanproductname").text() != "" || c.val().trim() == "") { return; } if (data.keyword != c.val()) { return; } if (data.rows == null || data.rows.length == 0) { d.hide(); noData = true; return; } noData = false; tempData[escape(requestByUrl("WarehouseId", options.url) + options.businame + c.val())] = data; if (options.header != null && t.find("thead").length == 0) { headerPropertys.length = 0; var thead = $("<thead></thead>"); for (var p in options.header) { if (typeof (options.header[p]) != "function") { thead.append("<th>" + options.header[p] + "</th>"); headerPropertys.push(p); } } t.append(thead); } t.find("tr").remove(); $.each(data.rows, function () { var tr = $("<tr></tr>"); tr.attr("rowValue", JsonToString(this)); if (headerPropertys.length > 0) { for (var i = 0; i < headerPropertys.length; i++) { if (headerPropertys[i].indexOf("canusestockcount") < 0 && headerPropertys[i].indexOf("currentstockcount") < 0) { if (this["cpProductUnitSBs"] != undefined && this["cpProductUnitSBs"] != "" && this["cpProductUnitSBs"] != null && this["cpProductUnitSBs"].length >= 1) { if (this["cpProductUnitSBs"].length > 1) { if (headerPropertys[i].indexOf("mainunitname") >= 0) { tr.append("<td style=\"background:#FFFFFF;border-bottom:soild 1px #d7d7d7;\"><div name='primarytop' class='primary direct'></div>" + KeyHighlight(this[headerPropertys[i]], c.val()) + "</td>"); } else { tr.append('<td style="background:#FFFFFF;border-bottom:soild 1px #d7d7d7;">' + KeyHighlight(this[headerPropertys[i]], c.val()) + "</td>"); } } else { if (headerPropertys[i].indexOf("mainunitname") >= 0) { tr.append("<td align='center' style=\"background:#FFFFFF;border-bottom:soild 1px #d7d7d7;\">" + KeyHighlight(this[headerPropertys[i]], c.val()) + "</td>"); } else { tr.append('<td style="background:#FFFFFF;border-bottom:soild 1px #d7d7d7;">' + KeyHighlight(this[headerPropertys[i]], c.val()) + "</td>"); } } } else { tr.append('<td style="background:#FFFFFF;border-bottom:soild 1px #d7d7d7;">' + KeyHighlight(this[headerPropertys[i]], c.val()) + "</td>"); } } else { tr.append('<td style="background:#FFFFFF;border-bottom:soild 1px #d7d7d7;">' + this[headerPropertys[i]] + "</td>"); } } } else { for (var p in this) { if (typeof (this[p]) != "function") { tr.append("<td>" + this[p] + "</td>"); } } } t.append(tr); }); var rows = t.find("tr"); rows.first().addClass("nowRow"); rows.first().find("td").css({ background: "#ecf4ff", "border-bottom": "soild 1px #bdd1eb" }); rows.mouseover(function () { $(this).find("td").css({ background: "#f4f4f4", "border-bottom": "soild 1px #bdd1eb" }); }); rows.mouseout(function () { var thisclass = $(this).attr("class"); if (thisclass == undefined || thisclass.indexOf("nowRow") < 0) { $(this).find("td").css({ background: "#ffffff", "border-bottom": "soild 1px #bdd1eb" }); } else { $(this).find("td").css({ background: "#ecf4ff", "border-bottom": "soild 1px #bdd1eb" }); } }); rows.click(function () { options.selected(StringToJson($(this).attr("rowValue"))); options.lastValue = ""; d.hide(); }); c.setBoxPosition(c, d, options.top); d.show(); if (options.isScanBar && data.rows != null && data.rows.length == 1) { var n = t.find(".nowRow"); if (n.length > 0) { options.selected(StringToJson(n.attr("rowValue"))); options.lastValue = ""; d.hide(); } } } function getData(o, process) { $.ajax({ type: options.type, async: options.async, url: options.url, data: { key: o.val() }, dataType: "json", cache: false, success: process, Error: function (err) { alert(err); } }); } }; function JsonToString(value) { return JSON.stringify(value); } function StringToJson(value) { eval("var jsonValue = " + value); return jsonValue; } function KeyHighlight(strSource, strKey) { if ("" == strKey.replace(/ /g, "").replace(/銆€/g, "")) { return strSource; } if (strKey == "+") { if (typeof strSource == "string") { return strSource.replace(/\+/g, "<span class='key'>+</span>"); } return strSource; } var htmlReg = new RegExp("<.*?>", "i"); var arrA = new Array(); for (var i = 0; true; i++) { var m = htmlReg.exec(strSource); if (m) { arrA[i] = m; } else { break; } strSource = strSource.replace(m, "{[(" + i + ")]}"); } words = unescape(strKey).split(/\s+/); for (w = 0; w < words.length; w++) { var r = new RegExp("(" + words[w].replace(/[(){}.+*?^$|\\\[\]]/g, "\\$&") + ")", "ig"); if (strSource == null) { strSource = ""; } strSource = strSource.toString().replace(r, "<span class='key'>$1</span>"); } for (var i = 0; i < arrA.length; i++) { strSource = strSource.replace("{[(" + i + ")]}", arrA[i]); } return strSource; } $.fn.resetEvent = function (c) { c.bind("keydown", keydown_process); c.bind("keyup", keyup_process); c.bind("blur", blur_process); d.bind("focus", focus_div); d.bind("mouseout", mouseout_div); }; $.fn.setBoxPosition = function (c, d, top) { var p = c.position(); d.css({ left: p.left, top: p.top + c.height() + top }); }; $.fn.setScroll = function (h, o, d) { var offset = o.offset(); var p = d.position(); if (offset.top - p.top > h - 2) { $(this).scrollTop(((offset.top - p.top - h) / 25 + 1) * 25 + $(this).scrollTop()); } else { if (offset.top - p.top < 25) { $(this).scrollTop(0); } } }; $.fn.setHeaderTop = function (h) { h.css("top", $(this).scrollTop()); }; })(jQuery);