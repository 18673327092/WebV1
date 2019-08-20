var assign = {
    values: [],
    names: [],
    init: function (options) {
        var _this = this;
        _this.options = { index: parent.layer.getFrameIndex(window.name) };
        $.extend(_this.options, options || {});
        _this._rendergrid();
        _this.bind();
        _this.initvalue();
    },
    initvalue: function () {
        var _this = this;
        $("#select_value>span").each(function (m, n) {
            var name = $(n).data("name");
            var id = $(n).data("id");
            _this.names.push(name);
            _this.values.push(id);
        });
    },
    select: function () {
        var _this = this;
        _this.values = [];
        _this.names = [];
        $("#select_value").html("");
        $.each(_this.grid.getcheckedrows(), function (z, data) {
            var tag = '<span class="select_span" data-name="' + data.Name + '" data-id="' + data.ID + '" ><span class="title"><i class="fa icon-ok-circle"></i>' + data.Name + '</span><input type="hidden" class="value" value="' + data.ID + '" /> </span>';
            var ishave = false;
            $.each(_this.values, function (m, n) {
                if (data.ID == n) { ishave = true; }
            });
            if (!ishave) {
                _this.values.push(data.ID);
                _this.names.push(data.Name);
                $("#select_value").append(tag);
            };
        });
        $(".select_span").on("click", function (e) {
            $(this).addClass("select_value_curr").siblings().removeClass("select_value_curr");
            $("#btn-remove").show();
        })
    },
    remove: function () {
        var _this = this;
        var select_value_curr = $(".select_value_curr");
        $.each(select_value_curr, function (m, n) {
            _this.values.remove($(n).find("input").val());
            _this.names.remove($(n).find("span").text())
        });
        select_value_curr.remove();
        $("#btn-remove").hide();
    },
    save: function () {
        var _this = this;
        var uid = 0;;
        var select_value = $("#select_value>span");
        for (var i = 0; i < select_value.length; i++) {
            uid = $("#select_value>span:eq(" + i + ")").attr("data-id");
        }
        if (uid == 0) { dlg.alert("请选择分派用户"); return; }
        dlg.confirm("确定将选中的数据分派给该用户吗？", function () {
            $.post("/Common/_SaveAssign", { v: request("v"), uid: uid }, function (data) {
                dlg.msg.info(data.Message, {});
                setTimeout(function () {
                    _this.cancel()
                }, 1000)
            }, "json");
        })
    },
    cancel: function () {
        var _this = this;
        parent.layer.close(_this.options.index);
    },
    bind: function () {
        var _this = this;
        $("#btn-save").on("click", function () { _this.save(); });
        $("#btn-cancel").on("click", function () { _this.cancel(); });
        $("#btn-remove,#btn-delete").on("click", function () { _this.remove(); });
        $("#btn-search").on("click", function () { _this.search(); });
        $("#btn-select").on("click", function () { _this.select(); });
        $("#KeyWord").keydown(function (event) {
            event = document.all ? window.event : event;
            if ((event.keyCode || event.which) == 13) { _this.search(); }
        });
    },
    _rendergrid: function () {
        var _this = this;
        //列表数据绑定
        _this.grid = new pagegride({
            id: "dicgrid",
            url: '/Common/_GetUserList',
            columns: [{ field: "ID", title: "ID", width: 50, hidden: true },
                { field: "Name", title: "姓名", width: 100 },
                { field: "RoleListText", title: "角色", width: 100 },
                { field: "Mobile", title: "手机号码", width: 100 }],
            params: { selectable: "multiple", },
            searchcontrols: [{ field: "KeyWord" }],
            pageSize: 50,
            ischeckbox: true,
            click: function () {
                _this.select();
            },
            dbclick: function () {
                _this.select();
                _this.save();
            }
        });
    }
}
