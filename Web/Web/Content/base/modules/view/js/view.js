; (function ($, window, document) {
    $.view = function (_option) {
        this.option = {
            container: "#query",
            entityid: 0,
            entityname: "",
            areaname: "",
            controllername: "",
            viewid: 0,
            userid: 0,
            viewname: "",
            viewremark: "",
            viewtype: "",
            viewismenu: 0,
            viewsort: "",
            previewgridid: "previewgrid",
            viewgridlistid: "grid",
            fields: [],
            fkentitys: [],
            dbfilter: [],
            columns: [],
            viewobj: null
        };

        $.extend(this.option, _option || {});
        this.sel_selectcontrol;
        this.span_selectcontrol;
        this.data;
        this.filtersql;
        this.previewobj = null;
        this.viewlistwobj = null;
        this.columnfields = [];
        this.dbfilter = { filters: [], fkentity: [] };
        this.currfilter = { express: "", value: "", title: "", filters: [] }
        this.init();
    }
    $.view.prototype = {
        //初始化
        init: function () {
            var _this = this;
            _this._init_filtercontrol();
            _this.bind();
            _this.loadfilter();
            //_this.loadviewlist();
            $.each(_this.option.columns, function (m, n) {
                _this.columnfields.push(n.field);
            })
        },
        //事件绑定
        bind: function () {
            var _this = this;
            $(_this.option.container).delegate('.span_select', 'mouseover', function () { $(this).hide(); $(this).prev().val(""); $(this).prev().show(); });
            $(_this.option.container).delegate('.selectcontrl', 'mouseout', function () { $(this).hide(); $(this).next().show(); });
            $(_this.option.container).delegate('select[id=btn_select]', 'change', function () { _this._selectcontrol_change(this); });
            $(_this.option.container).delegate('select[class*=fieldselectcontrol]', 'change', function () { _this._render_express(this); });
            $(_this.option.container).delegate('select[class*=express]', 'change', function () { _this._render_value(this) });
            $(_this.option.container).delegate('.fa-trash-o', 'click', function () { $(this).parent().parent().remove(); });
            $(_this.option.container).delegate('.select_dictionvalue', 'click', function () { _this._open_dictionary_dialog(this); });
            $(_this.option.container).delegate('.select_filedvalue', 'click', function () { _this._open_field_dialog(this); });
        },
        //初始化条件选择控件
        _init_filtercontrol: function () {
            var _this = this;
            var panel = $('<div class="pane" id="base_pane">\
                                <div style="width:120px;">\
                                    <select style="display: none; margin-bottom: 3px;" class="form-control selectcontrl" id="btn_select">\
                                        <optgroup label="选择">\
                                            <option value="">选择筛选字段</option>\
                                        </optgroup>\
                                        <optgroup label="相关"></optgroup>\
                                    </select>\
                                    <div class="form-span span_select" style="border: 0px; margin-bottom: 3px;"><a href="javascript://"  >选择筛选字段</a></div>\
                                </div>\
                            </div>');
            $(_this.option.container).append(panel);
            _this.sel_selectcontrol = panel.find("#btn_select");
            _this.span_selectcontrol = panel.find("#span_select");
            //实体本身的字段
            $.each(_this.option.fields, function (m, n) {
                _this.sel_selectcontrol.find("optgroup:eq(0)").append('<option value="' + n.Name + '" data-fieldid="' + n.ID + '" data-type="' + n.fieldType + '" >' + n.title + '</option>');
            });
            //实体的外键
            $.each(_this.option.fkentitys, function (m, n) {
                $.each(n.Fileds, function (z, s) {
                    s["Template"] = null;
                    s["TemplateSql"] = null;
                })
                _this.sel_selectcontrol.find("optgroup:eq(1)")
                    .append('<option data-value="' + n.Value
                    + '" value="' + n.Value + '" data-showname="' + n.ShowName
                    + '" data-type="entity" data-jointablename="' + n.JoinTableName + '" data-onsql="' + n.OnSql
                    + '" data-entityid="' + n.RelationEntityID + '" data-fields=\'' + JSON.stringify(n.Fileds) + '\' >' + n.ShowName + '</option>');
            })
            _this.sel_selectcontrol.change();
        },
        //条件选择框选择事件
        _selectcontrol_change: function (e) {
            var _this = this;
            var $check = $(e).find("option:checked");
            var check = {
                type: $check.data("type"),
                value: $check.val(),
                fieldid: $check.data("fieldid"),
                entityid: $check.data("entityid")
            };
            // console.log(JSON.stringify(check));
            $(e).hide();
            $(e).next().show();
            if (check.type != "entity") {
                //选中的是字段
                _this._render_field(e, _this.option.fields);
            }
            else if (check.type == "entity") {
                //选中的是实体
                _this._render_fkentity(e, $(e).find("option:checked").data())
            }
        },
        //渲染条件选择框-选择字段
        _render_field: function (e, fields) {
            var _this = this;
            var $check = $(e).find("option:checked");
            var check = {
                type: $check.data("type"),
                value: $check.val(),
                fieldid: $check.data("fieldid"),
                level: $check.data("level")
            };
            var $group = $('<div class="groupLine explist" style="width:1000px;clear:both;height: 31px;">\
                                <div class="query1"><select class="form-control fieldselectcontrol"></select></div>\
                                <div class="query2"><select class="form-control express" ></select></div>\
                                <div class="query3"></div>\
                                <div style="float:left;width:30px"><i class="fa fa-trash-o"></i></div>\
                            </div>');
            //其他实体字段触发
            if (check.level == 2) {
                $group = $('<div class="groupLine relationexplist" style="width:1000px;clear:both;height: 31px;margin-left: 15px;">\
                                <div class="query1"><select class="form-control fieldselectcontrol"></select></div>\
                                <div class="query2"><select class="form-control express" ></select></div>\
                                <div class="query3" style="width:285px"></div>\
                                <div style="float:left;width:30px"><i class="fa fa-trash-o"></i></div>\
                            </div>');
            }
            $(e).parent().before($group);
            var $field_select = $group.find(".query1").find("select");
            $.each(fields, function (m, n) {
                $field_select.append('<option data-type="' + n.fieldType + '"  data-level="1" data-fieldid="' + n.ID + '" value="' + n.Name + '" >' + n.title + '</option>');
            })
            $field_select.val(check.value);
            _this._render_express($field_select);
            // if (check.type != "日期" && check.type != "时间") {
            //_this._render_value($field_select)

            //}
        },
        //渲染条件选择框-选择实体
        _render_fkentity: function (e, data) {
            var _this = this;
            var entityname = $(e).find("option:checked").val();
            console.log(JSON.stringify(data));
            var $panel = $('<div class="relation_entity_panel">\
                                    <div style="background-color:#F5F5F5;padding: 3px 10px;float:left;width: 97%;" class="form-control relation_entity"></div><div style="float:left;width:30px"> <i class="fa fa-trash-o"></i></div>\
                                    <div style="clear:both;">\
                                        <select style="display: none;margin-left: 15px;margin-bottom: 3px;width:120px;"  class="form-control selectcontrl fkentity_selectcontrl">\
                                        <option value="">请选择</option></select>\
                                        <div class="form-span span_select" style=" margin-left: 15px; width: 120px;border: 0; margin-bottom: 3px; "><a href="javascript://">选择</a></div>\
                                    </div>\
                                </div>');
            $("#query").append($panel);
            var fkentity_selectcontrl = $panel.find(".fkentity_selectcontrl");
            var $relationEntity = $panel.find("div[class*=relation_entity]");
            for (var r in data) { $relationEntity.data(r.toLowerCase(), data[r]); }
            $relationEntity.html(data.showname);
            $.each(data.fields, function (m, n) {
                fkentity_selectcontrl.append('<option value="' + n.Name + '" data-level="2" data-fieldid="' + n.ID + '" data-type="' + n.fieldType + '" >' + n.title + '</option>');
            })
            fkentity_selectcontrl.change(function () {
                _this._render_field(this, data.fields)
            });
            if (_this.currfilter.filters) {
                $.each(_this.currfilter.filters, function (m, n) {
                    _this.currfilter = n;
                    fkentity_selectcontrl.val(n.name).change();
                });
            }
        },
        //渲染表达式
        _render_express: function (e) {
            var _this = this;
            var expression;
            var $check = $(e).find("option:checked");
            var check = {
                type: $check.data("type"),
                value: $check.val(),
                fieldid: $check.data("fieldid"),
                level: $check.data("level")
            };
            switch (check.type) {
                case "整数":
                case "货币":
                case "浮点型":
                    expression = [
                         { value: "{0} ={1}", type: "浮点型", title: "等于" },
                         { value: "{0} !={1}", type: "浮点型", title: "不等于" },
                         { value: "{0} >={1}", type: "浮点型", title: "大于等于" },
                         { value: "{0} >{1}", type: "浮点型", title: "大于" },
                         { value: "{0} <={1}", type: "浮点型", title: "小于等于" },
                         { value: "{0} <{1}", type: "浮点型", title: "小于" },
                    ];
                    break;
                case "单行文本":
                case "多行文本":
                    expression = [
                        { value: "{0} ='{1}'", type: "多行文本", title: "等于" },
                        { value: "{0} !='{1}'", type: "多行文本", title: "不等于" },
                        { value: "{0} like '{1}%'", type: "多行文本", title: "开头为" },
                        { value: "{0} like '%{1}%'", type: "多行文本", title: "包含" },
                        { value: "{0} not like '%{1}%'", type: "多行文本", title: "不包含" },
                        { value: "{0} like '%{1}'", type: "多行文本", title: "结尾为" },

                    ];
                    break;
                case "日期":
                    expression = [
                        { value: "{0} BETWEEN '{1} 00:00:00' AND '{1} 23:59:59'", type: "日期", title: "等于" },
                        { value: "{0} NOT BETWEEN '{0} 00:00:00' AND '{0} 23:59:59'", type: "日期", title: "不等于" },
                        { value: "{0} >='{1} 00:00:00'", type: "日期", title: "大于等于" },
                        { value: "{0} >'{1}  23:59:59'", type: "日期", title: "大于" },
                        { value: "{0} <='{1} 23:59:59'", type: "日期", title: "小于等于" },
                        { value: "{0} <'{1}  00:00:00'", type: "日期", title: "小于" },
                        { value: "{0} BETWEEN convert(varchar(10),getdate(),120) AND convert(varchar(10),getdate(),120)+' 23:59:59'", title: "今天" },
                        { value: "{0} BETWEEN convert(varchar(10),getdate()+1,120) AND convert(varchar(10),getdate()+1,120)+' 23:59:59'", title: "明天" },
                        { value: "{0} BETWEEN convert(varchar(10),getdate()-{1},120) AND convert(varchar(10),getdate(),120)+' 23:59:59'", type: "整数", title: "最近 X 天以内" },
                        { value: "{0} BETWEEN convert(varchar(10),getdate(),120) AND convert(varchar(10),getdate()+{1},120)+' 23:59:59'", type: "整数", title: "今后 X 天以内" },
                        { value: "{0} >= convert(varchar(10),getdate()+{1},120)", type: "整数", title: "大于等于 X 天以后" },
                        { value: "{0} <= convert(varchar(10),getdate()+{1},120)+' 23:59:59'", type: "整数", title: "小于等于 X 天以后" },
                        { value: "DATEDIFF(wk, {0}, GetDate())=1", title: "上周" },
                        { value: "DATEDIFF(wk, {0}, GetDate())=0", title: "本周" },
                        { value: "DATEDIFF(wk, {0}, GetDate())=-1", title: "下周" },
                        { value: "DATEDIFF(wk, {0}, GetDate())={1}", type: "整数", title: "最近 X 周" },
                        { value: "DATEDIFF(wk, {0}, GetDate())=-{1}", type: "整数", title: "今后 X 周" },
                        { value: "DATEDIFF(MM, {0}, GetDate())=1", title: "上个月" },
                        { value: "DATEDIFF(MM, {0}, GetDate())=0", title: "本月" },
                        { value: "DATEDIFF(MM, {0}, GetDate())=-1", title: "下个月" },
                        { value: "DATEDIFF(MM, {0}, GetDate())={1}", type: "整数", title: "最近 X 个月" },
                        { value: "DATEDIFF(MM, {0}, GetDate())=-{1}", type: "整数", title: "今后 X 个月" },
                        { value: "DATEDIFF(YY, {0}, GetDate())=0", title: "今年" },
                        { value: "DATEDIFF(YY, {0}, GetDate())={1}", type: "整数", title: "最近 X 年" },
                        { value: "DATEDIFF(YY, {0}, GetDate())=-{1}", type: "整数", title: "今后 X 年" },
                    ];
                    break;
                case "时间":
                    expression = [
                        { value: "{0} BETWEEN '{1}' AND '{1}'", type: "时间", title: "等于" },
                        { value: "{0} NOT BETWEEN '{1}' AND '{1}'", type: "时间", title: "不等于" },
                        { value: "{0} >='{1}'", type: "时间", title: "大于等于" },
                        { value: "{0} >'{1}'", type: "时间", title: "大于" },
                        { value: "{0} <='{1}'", type: "时间", title: "小于等于" },
                        { value: "{0} <'{1}'", type: "时间", title: "小于" },
                        { value: "{0} BETWEEN convert(varchar(10),getdate(),120) AND convert(varchar(10),getdate(),120)+' 23:59:59'", title: "今天" },
                        { value: "{0} BETWEEN convert(varchar(10),getdate()+1,120) AND convert(varchar(10),getdate()+1,120)+' 23:59:59'", title: "明天" },
                        { value: "{0} BETWEEN convert(varchar(10),getdate()-{1},120) AND convert(varchar(10),getdate(),120)+' 23:59:59'", type: "整数", title: "最近 X 天以内" },
                        { value: "{0} BETWEEN convert(varchar(10),getdate(),120) AND convert(varchar(10),getdate()+{1},120)+' 23:59:59'", type: "整数", title: "今后 X 天以内" },
                        { value: "{0} >= convert(varchar(10),getdate()+{1},120)", type: "整数", title: "大于等于 X 天以后" },
                        { value: "{0} <= convert(varchar(10),getdate()+{1},120)+' 23:59:59'", type: "整数", title: "小于等于 X 天以后" },
                        { value: "DATEDIFF(wk, {0}, GetDate())=1", title: "上周" },
                        { value: "DATEDIFF(wk, {0}, GetDate())=0", title: "本周" },
                        { value: "DATEDIFF(wk, {0}, GetDate())=-1", title: "下周" },
                        { value: "DATEDIFF(wk, {0}, GetDate())={1}", type: "整数", title: "最近 X 周" },
                        { value: "DATEDIFF(wk, {0}, GetDate())=-{1}", type: "整数", title: "今后 X 周" },
                        { value: "DATEDIFF(MM, {0}, GetDate())=1", title: "上个月" },
                        { value: "DATEDIFF(MM, {0}, GetDate())=0", title: "本月" },
                        { value: "DATEDIFF(MM, {0}, GetDate())=-1", title: "下个月" },
                        { value: "DATEDIFF(MM, {0}, GetDate())={1}", type: "整数", title: "最近 X 个月" },
                        { value: "DATEDIFF(MM, {0}, GetDate())=-{1}", type: "整数", title: "今后 X 个月" },
                        { value: "DATEDIFF(YY, {0}, GetDate())=0", title: "今年" },
                        { value: "DATEDIFF(YY, {0}, GetDate())={1}", type: "整数", title: "最近 X 年" },
                        { value: "DATEDIFF(YY, {0}, GetDate())=-{1}", type: "整数", title: "今后 X 年" },
                    ];
                    break;
                case "两个选项":
                case "选项集":
                    expression = [
                        { value: "{0} IN({1})", type: "选项集", title: "等于" },
                        { value: "{0} NOT IN({1})", type: "选项集", title: "不等于" }
                    ];
                    break;
                case "关联其他表":
                    expression = [
                         { value: "{0} IN({1})", type: "关联其他表", title: "等于" },
                         { value: "{0} NOT IN({1})", type: "关联其他表", title: "不等于" }
                    ];
                    break;
            }
            var $group = $(e).parent("div").parent("div");
            var $express_select = $group.find(".query2").find("select");
            $express_select.find("option").remove();
            expression.push({ value: "({0} IS NULL OR {0}='')", title: "等于空值" });
            expression.push({ value: "{0} IS NOT NULL AND {0}<>''", title: "不等于空值" });
            $.each(expression, function (m, n) {
                $express_select.append('<option value="' + n.value + '" data-level="' + check.level + '" data-fieldid="' + check.fieldid + '" data-type="' + n.type + '" >' + n.title + '</option>');
            })
            if (_this.currfilter.express) {
                $express_select.val(_this.currfilter.express);
            }
            _this._render_value($express_select);
        },
        //渲染条件输入框
        _render_value: function (e) {
            var _this = this;
            var control = '';
            var $check = $(e).find("option:checked");
            var check = {
                type: $check.data("type"),
                value: $check.val(),
                fieldid: $check.data("fieldid")
            };
            switch (check.type) {
                case "整数":
                    control = '<input class="form-tag filed form-field form-control numberbox"  maxlength="8" type="text" placeholder="输入整数" value="' + _this.currfilter.value + '" />';
                    break;
                case "货币":
                    control = '<input class="form-tag filed form-field form-control numberbox"  maxlength="8" type="text" placeholder="输入有1~3位小数的正实数" value="' + _this.currfilter.value + '" />';
                    break;
                case "浮点型":
                    control = '<input class="form-tag filed form-field form-control numberbox"  maxlength="8" type="text" placeholder="输入有1~3位小数的正实数" value="' + _this.currfilter.value + '" />';
                    break;
                case "单行文本":
                case "多行文本":
                    control = '<input class="form-tag filed form-field form-control text-box single-line" value="' + _this.currfilter.value + '" type="text" placeholder="输入文本" />';
                    break;
                case "时间":
                    control = '<input onclick="laydate({istime: true, format: \'YYYY-MM-DD hh:mm:ss\'})" value="' + _this.currfilter.value + '" class="form-tag filed form-control text-box single-line laydate-icon time" >';
                    break;
                case "日期":
                    control = '<input onclick="laydate({istime: false, format: \'YYYY-MM-DD\'})" value="' + _this.currfilter.value + '" class="form-tag filed form-control text-box single-line laydate-icon date" >';
                    break;
                case "两个选项":
                case "选项集":
                    control = '<span class="input-icon">\
                                    <span>' + _this.currfilter.title + '</span>\
                                    <i class="icon-search nav-search-icon select_dictionvalue" data-fieldid="' + check.fieldid + '" data-value="' + _this.currfilter.value + '"  style="cursor: pointer; top: -1px;"></i>\
                               </span>';
                    break;
                case "关联其他表":
                    var _id = Math.floor(Math.random() * 100);
                    control = '<span class="input-icon">\
                                    <span id="span_' + check.fieldid + _id + '">' + _this.currfilter.title + '</span>\
                                    <i class="icon-search nav-search-icon select_filedvalue" data-id="' + _id + '" id="icon_' + check.fieldid + _id + '"  data-value="' + _this.currfilter.value + '"  data-fieldid="' + check.fieldid + '" style="cursor: pointer; top: -1px;"></i>\
                               </span>';
                    break;
            }

            var $group = $(e).parent("div").parent("div");
            $group.find(".query3").html(control);
        },
        //弹出选项集选择框
        _open_dictionary_dialog: function (e) {
            var value = $(e).data("value");
            var fieldid = $(e).data("fieldid");
            dlg.openPageHtml({
                id: "dic",
                url: "/View/DictionarySelectPanel?fieldid=" + fieldid + "&value=" + ((typeof value) != "undefined" ? value : ""),
                area: ["480px"]
            }, function () {
                var title = [];
                var value = [];
                $.each($("#ul_right").find("li"), function (m, n) {
                    title.push($(n).html())
                    value.push($(n).data("value"))
                })
                $(e).data("value", value.join(','));
                $(e).prev().html(title.join(","));
                var index = layer.index; //获取当前弹层的索引号
                layer.close(index); //关闭当前弹层
            });
        },
        //弹出关联字段值选择框
        _open_field_dialog: function (e) {
            var value = $(e).data("value");
            var fieldid = $(e).data("fieldid");
            var id = $(e).data("id");
            dlg.openframe({
                title: "选择",
                offset: ['10px'],
                type: 2,
                //btn: ['确定', '取消'],
                //shade: 0,
                area: ['600px', '500px'],
                fixed: true, //不固定
                maxmin: false,
                content: "/View/RelationFieldSelectPanel?t=0&fieldid=" + fieldid + "&value=" + ((typeof value) != "undefined" ? value : "") + "&id=" + id,
                yes: function (index) {
                    parent.layer.close(index);
                },
            });
        },
        //获取高级查找所需要的sql语句
        _get_viewsql: function () {
            var _this = this;
            var viewsql = {
                entityid: _this.option.entityid,
                entityname: _this.option.entityname,
                filtersql: "",
                relationentity: [],
                express: ""
            };
            var dbexpress = [];
            var filter = [];
            //基本属性
            var basefield = [];
            $.each($(".explist"), function (m, n) {
                var field = $(n).find(".query1").find("select").val();
                var type = $(n).find(".query1").find("select").find("option:checked").data("type");
                var express = $(n).find(".query2").find("select").val();
                var value = $(n).find(".query3").find("input").val();
                var title = $(n).find(".query3").find("i").prev("span").html();
                if (type == "选项集" || type == "两个选项" || type == "关联其他表") {
                    value = $(n).find(".query3").find("i").data("value");
                }
                filter.push($.format(express, _this.option.entityname + "." + field, value));
                dbexpress.push({ name: field, express: express, value: value, title: title });
            });

            //相关属性
            $.each($(".relation_entity_panel"), function (m, n) {
                var $relation_entity = $(n).find("div[class*=relation_entity]");
                viewsql.relationentity.push({
                    jointablename: $relation_entity.data("jointablename"),
                    onsql: $relation_entity.data("onsql")
                });
                var fkexpress = { name: $relation_entity.data("value"), type: "entity", title: $relation_entity.data("showname"), filters: [] }
                var relationfield = [];

                $.each($(n).find(".relationexplist"), function (m, n) {
                    var field = $(n).find(".query1").find("select").val();
                    var type = $(n).find(".query1").find("select").find("option:checked").data("type");
                    var express = $(n).find(".query2").find("select").val();
                    var value = $(n).find(".query3").find("input").val();
                    var title = $(n).find(".query3").find("i").prev("span").html();
                    if (type == "选项集" || type == "两个选项" || type == "关联其他表") {
                        value = $(n).find(".query3").find("i").data("value");
                    }
                    //filter.push($relation_entity.data("value") + "." + field + " " + $.format(express, value));
                    filter.push($.format(express, $relation_entity.data("value") + "." + field, value));
                    fkexpress.filters.push({
                        name: field,
                        express: express,
                        value: value,
                        title: title
                    });
                });
                dbexpress.push(fkexpress);
            });
            viewsql.filtersql = filter.join(" AND ");
            viewsql.express = JSON.stringify(dbexpress);
            viewsql.viewid = _this.option.viewid;
            return viewsql;
        },
        //预览
        preview: function () {
            var _this = this;
            var _filtersql = _this._get_viewsql().filtersql;
            if (_filtersql == _this.filtersql && _this.previewobj && _this.previewobj.grid) {
                var grid = _this.previewobj.grid.data('kendoGrid');
                grid.dataSource.page(1);
                return;
            }
            _this.filtersql = _filtersql;
            $.each(_this.option.columns, function (m, n) {
                if (n["template"] != "" && n["template"] != null) {
                    n["template"] = escape(n["template"]);
                }
            })
            $.post("/View/PreviewSubmit", { JsonViewSaveEntity: JSON.stringify(_this._get_viewsql()), JsonField: JSON.stringify(_this.option.columns) }, function (data) {
                if (data.Success) {
                    $.each(_this.option.columns, function (m, n) {
                        if (n["template"] != "" && n["template"] != null) {
                            n["template"] = unescape(n["template"]);
                        }
                    })

                    // console.log(JSON.stringify({ key: data.data, sql: data.sql, filtersql: _filtersql }))
                    if (_this.data == data.data) { return false; }
                    _this.data = data.data;
                    $("#key").val(data.data);
                    if (_this.previewobj != null) {
                        var grid = _this.previewobj.grid.data('kendoGrid');
                        grid.dataSource.page(1);
                    } else {
                        var _columns = [];
                        $.extend(_columns, _this.option.columns || {});
                        var c = _columns;
                        $.each(c, function (m, n) {
                            c.width = parseFloat(n.width) + 50;
                            if (n["template"] != "" && n["template"] != null) {
                                n["template"] = unescape(n["template"]);
                            }
                        })

                        _this.previewobj = new pagegride({
                            id: _this.option.previewgridid,
                            url: "/View/_PreviewList",
                            ischeckbox: false,
                            sortable: true,
                            searchcontrols: [{ field: "key" }],
                            paramsdata: { eid: _this.option.entityid },
                            columns: _this.option.columns,
                            dbclick: function (data) {
                                if (_this.option.areaname != "") {
                                    openwindow("/" + _this.option.areaname + "/" + _this.option.controllername + "/Form/" + data[_this.option.entityname + "$ID"], 0, 40)
                                } else {
                                    openwindow("/" + _this.option.controllername + "/Form/" + data[_this.option.entityname + "$ID"], 0, 40)
                                }
                                // parent.dlg.openframe({ title: data[_this.option.entityname + "$Name"], content:  })
                            }
                        });
                    }

                } else {

                }
            });

            $("#previewgrid").height($(window).height() - 110);
            $("#grid").height($(window).height() - 110);
        },
        //保存
        _save: function (t, callback) {
            var _this = this;
            //console.log(JSON.stringify(_this._get_viewsql()));
            var viewname = _this.option.viewname;
            var viewremark = _this.option.viewremark;
            var viewid = _this.option.viewid;
            //另存为
            if (t == 2) {
                viewname = "";
                viewremark = "";
                viewid = 0;
            }
            dlg.openContent({
                id: "dic",
                title: "",
                offset: ['20px'],
                content: '<fieldset style="overflow-y:auto;margin:15px;">\
                            <div class="form-group">\
                                <label class="control-label" for="" title="">视图名称：</label>\
                                <div class="control-div">\
                                    <input type="text" class="form-control" placeholder="视图名称"  id="ViewTitle" value="' + viewname + '">\
                                </div>\
                            </div>\
                           <div class="form-group" id="viewtype" style="display:' + (_this.option.userid == 999 ? "display" : "none") + ';">\
                                <label class="control-label" for="" title="">视图类型：</label>\
                                <div class="control-div">\
                                    <select name="Type" id="sel_type" class="form-control">\
                                        <option value="自定义视图" ' + (_this.option.viewtype == "自定义视图" ? "selected='selected'" : "") + ' >自定义视图</option>\
                                        <option value="系统视图" ' + (_this.option.viewtype == "系统视图" ? "selected='selected'" : "") + '>系统视图</option>\
                                        <option value="关联视图" ' + (_this.option.viewtype == "关联视图" ? "selected='selected'" : "") + '>关联视图</option>\
                                        <option value="弹框视图" ' + (_this.option.viewtype == "弹框视图" ? "selected='selected'" : "") + '>弹框视图</option>\
                                        <option value="首页展示视图" ' + (_this.option.viewtype == "首页展示视图" ? "selected='selected'" : "") + '>首页展示视图</option>\
                                    </select>\
                                </div>\
                            </div>\
                            <div class="form-group" style="display:' + (_this.option.userid == 999 ? "display" : "none") + ';">\
                                        <label class="control-label" for="" title="">属性：</label>\
                                        <div class="control-div ">\
                                            <div class="checkbox" style="margin: 0px !important; padding: 0px;padding-left: 7px;">\
                                                <label class="checkbox-inline" style="display: inline-block">\
                                                    <input style="margin: 5px -15px;" id="IsMenu" name="IsMenu" ' + (_this.option.viewismenu == 1 ? "checked='checked' " : "") + ' type="checkbox" value="true">工作提醒\
                                                </label>\
                                            </div>\
                                        </div>\
                                    </div>\
                            <div class="form-group">\
                                <label class="control-label" for="" title="">备注：</label>\
                                <div class="control-div">\
                                    <textarea id="ViewRemark" cols="20" style="height:100px" class="form-control">' + viewremark + '</textarea>\
                                </div>\
                            </div>\
                        </fieldset>',
                area: ["473px"]
            }, function () {
                var index = layer.index; //获取当前弹层的索引号
                var viewobj = {
                    ID: viewid,
                    Title: $("#ViewTitle").val().trim(),
                    Type: $("#sel_type").val().trim(),
                    Remark: $("#ViewRemark").val().trim(),
                    IsMenu: $("#IsMenu").is(":checked"),
                    Sort: _this.option.viewsort,
                };
                if (viewobj.Title == "") { dlg.msg.info("视图名称不能为空"); $("#ViewTitle").focus(); return false; }
                $.each(_this.option.columns, function (m, n) {
                    if (n["template"] != "" && n["template"] != null) {
                        n["template"] = escape(n["template"]);
                    }
                })
                // console.log(JSON.stringify(viewobj));
                $.post("/View/Submit", { JsonViewSaveEntity: JSON.stringify(_this._get_viewsql()), JsonView: JSON.stringify(viewobj), JsonField: JSON.stringify(_this.option.columns) },
                    function (data) {
                        if (data.Success) {
                            _this.viewlistwobj = null;
                            _this.loadviewlist();
                            dlg.msg.info("视图保存成功");
                            setTimeout(function () {
                                if (parent.layer)
                                    parent.layer.closeAll();
                                layer.closeAll();
                                //  layer.close(index); //关闭当前弹层
                            }, 1000);;
                            //dlg.msg.info("视图保存成功",  { time: 1000 },
                            //    function () {
                            //       layer.close(index); //关闭当前弹层
                            //    });

                        } else {
                            dlg.msg.info("视图保存失败");
                            layer.close(index); //关闭当前弹层
                            // console.log(data.Message);
                        }
                    });
            });
        },
        deleteview: function () {
            var _this = this;
            var id = _this.viewlistwobj.getcheckedrows()[0].ID
            if (id > 0) {
                dlg.confirm("确定删除选中的视图吗？删除后不能恢复", function () {
                    $.post("/View/_DeleView", { id: id }, function (data) {
                        if (data.Success) {
                            dlg.msg.info("删除成功");
                            _this.viewlistwobj = null;
                            _this.loadviewlist();
                        } else {
                            dlg.msg.info("删除失败");
                        }
                    })
                });
            }

        },
        save: function (callback) {
            this._save(1, function () { })
        },
        saveto: function () {
            this._save(2, function () { })
        },
        //加载视图筛选条件
        loadfilter: function () {
            var _this = this;
            if (_this.option.dbfilter.length > 0) {
                _this.sel_selectcontrol.show();
                _this.span_selectcontrol.hide();
                $.each(_this.option.dbfilter, function (m, n) {
                    _this.currfilter = n;
                    _this.sel_selectcontrol.val(n.name).change();
                });
                _this.currfilter = { express: "", value: "", title: "" }
            }
        },
        loadviewlist: function () {
            var _this = this;
            if (_this.viewlistwobj != null) return false;
            _this.viewlistwobj = new pagegride({
                id: _this.option.viewgridlistid,
                url: "/View/GetViewList",
                ischeckbox: false,
                paramsdata: { eid: _this.option.entityid },
                sortable: true,
                columns: [
                           {
                               field: "Title",
                               title: "名称",
                               width: 180,
                           },
                           {
                               field: "UpdateTime",
                               title: "最近修改时间",
                               width: 180
                           },
                           {
                               field: "Remark",
                               title: "备注",
                               width: 796
                           }

                ],
                click: function (data) {
                    $("#btn-view-delete").show();
                },
                dbclick: function (data) {
                    //    console.log(JSON.stringify(data));
                    if (parent.layer)
                        parent.layer.closeAll();
                    layer.closeAll();
                    parent.dlg.openframe({ title: "自定义视图【" + data.Title + "】", content: "/View?eid=" + data.EntityID + "&v=" + data.ID })
                }
            });


        },
        //编辑列
        editcolumns: function () {
            var _this = this;
            $.each(_this.option.columns, function (m, n) {
                if (n["template"] != "" && n["template"] != null) {
                    n["template"] = escape(n["template"]);
                }
            })

            $.post("/View/SendViewFields", { viewfields: JSON.stringify(_this.option.columns), viewsort: _this.option.viewsort }, function (data) {
                dlg.openframe({
                    title: "选择",
                    //  offset: ['10px'],
                    btn: ['确定', '取消'],
                    //shade: 0,
                    area: ['833px', '463px'],
                    //fixed: false, //不固定
                    //maxmin: true,
                    //move: true,
                    content: "/View/EditColumns?eid=" + _this.option.entityid + "&key=" + data.data,
                    yes: function (index) {
                        var columns = $(layer.getChildFrame("#columns_container", index))
                        var fielddata = JSON.parse($(layer.getChildFrame("#fielddata", index)).val())
                        _this.option.viewsort = $(layer.getChildFrame("#viewsort", index)).val()
                        
                        _this.columnfields = [];
                        _this.option.columns = [];
                        $.each(fielddata, function (m, n) {
                            if (n["template"] != "" && n["template"] != null) {
                                n["template"] = unescape(n["template"]);
                                n["template"] = unescape(n["template"]);
                            }
                            //if (_this.columnfields.indexOf(n.field) == -1) {
                            _this.columnfields.push(n.field);
                            _this.option.columns.push(n);
                            //}
                        });
                        $("#" + _this.option.previewgridid).removeAttr("data-role").html("");
                        _this.filtersql = null;
                        _this.data = null;
                        _this.previewobj = null;
                        _this.preview();
                        layer.close(index);
                    },
                })
            }, "json")

        }
    }
})($, window, document);

$(function () {
    $("#btn-view-delete").hide();
    $("#btn-view-close").click(function () {
        parent.layer.closeAll()
    });
})
layui.use('element', function () {
    var $ = layui.jquery
    , element = layui.element(); //Tab的切换功能，切换事件监听等，需要依赖element模块

    //触发事件
    var active = {
        tabAdd: function () {
            //新增一个Tab项
            element.tabAdd('demo', {
                title: '新选项' + (Math.random() * 1000 | 0) //用于演示
              , content: '内容' + (Math.random() * 1000 | 0)
            })
        }
      , tabDelete: function () {
          //删除指定Tab项
          element.tabDelete('demo', 2); //删除第3项（注意序号是从0开始计算）
      }
      , tabChange: function () {
          //切换到指定Tab项
          element.tabChange('demo', 1); //切换到第2项（注意序号是从0开始计算）
      }
    };

    $('.site-demo-active').on('click', function () {
        var type = $(this).data('type');
        active[type] ? active[type].call(this) : '';
    });
});
var view = new $.view({
    container: "#query",
    entityid: $("#entityid").val(),
    entityname: $("#entityname").val(),
    areaname: $("#areaname").val(),
    controllername: $("#controllername").val(),
    viewid: $("#viewid").val(),
    userid: $("#userid").val(),
    viewname: $("#viewname").val(),
    viewsort: $("#viewsort").val(),
    viewismenu: $("#viewismenu").val(),
    viewremark: $("#viewremark").val(),
    viewtype: $("#viewtype").val(),
    previewgridid: "previewgrid",
    viewgridlistid: "grid",
    fields: JSON.parse($("#fieldlist").val()),
    fkentitys: JSON.parse($("#fkentitylist").val()),
    columns: JSON.parse($("#viewfieldlist").val()),
    dbfilter: $("#express").val() == "" ? [] : JSON.parse($("#express").val())
});

//关联弹框
function OpenValue(value) {
    var link = value.toString().split("$")[0] + "/" + value.toString().split("$")[1];
    var id = value.toString().split("$")[2];
    var name = value.toString().split("$")[3];
    // location.href = "/" + link + "/" + id;
    // openwindow("/" + link + "/Form/" + id)
    parent.dlg.openframe({ title: name, content: "/" + link + "/" + id })
}

function totips(value) {
    if (value.toString().length > 10) {
        return '<span class="tooltip" title="' + value + '">' + (value.toString().substring(0, 10) + "...") + '</span>';
    } else { return value; }
}