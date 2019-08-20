var rolelist = {
    values: [],
    names: [],
    init: function (options) {
        var _this = this;
        _this.options = {
            id: "", uid: 0, index: parent.layer.getFrameIndex(window.name),
        };
        $.extend(_this.options, options || {});
        _this._rendergrid();
        _this.bind();
        _this.initvalue()

    }
	, initvalue: function () {
	    var _this = this;
	    $("#select_value>span").each(function (m, n) {
	        var name = $(n).data("name");
	        var id = $(n).data("id");
	        _this.names.push(name);
	        _this.values.push(id)

	    });
	    $(".select_span").on("click", function (e) {
	        $(this).addClass("select_value_curr").siblings().removeClass("select_value_curr");
	        $("#btn-remove").show()

	    })

	}
	, select: function () {
	    var _this = this;
	    $.each(_this.grid.getcheckedrows(), function (z, data) {
	        var tag = '<span class="select_span" data-name="' + data.Name + '" data-id="' + data.ID + '" ><span class="title"><i class="fa icon-ok-circle"></i>' + data.Name + '</span><input type="hidden" class="value" value="' + data.ID + '" /> </span>';
	        var ishave = false;
	        $.each(_this.values, function (m, n) {
	            if (data.ID == n) ishave = true

	        });
	        if (!ishave) {
	            _this.values.push(data.ID);
	            _this.names.push(data.Name);
	            $("#select_value").append(tag)

	        }
	    });
	    $(".select_span").on("click", function (e) {
	        $(this).addClass("select_value_curr").siblings().removeClass("select_value_curr");
	        $("#btn-remove").show()

	    })

	}
	, remove: function () {
	    var _this = this;
	    var select_value_curr = $(".select_value_curr");
	    $.each(select_value_curr, function (m, n) {
	        _this.values.remove($(n).find("input").val());
	        _this.names.remove($(n).find("span").text())

	    });
	    select_value_curr.remove();
	    $("#btn-remove").hide()

	}
	, save: function () {
	    var _this = this;
	    var roles = [];
	    var rolesname = [];
	    var select_value = $("#select_value>span");
	    for (var i = 0; i < select_value.length; i++) {
	        roles.push($("#select_value>span:eq(" + i + ")").attr("data-id"));
	        rolesname.push($("#select_value>span:eq(" + i + ")").attr("data-name"))
	    };
	    $.post("/User/_SetRole", {
	        roles: roles.join(","), UID: _this.options.uid, rolesname: rolesname.join(",")
	    }
		, function (data) {
		    if (data.Success) {
		        dlg.alert("保存成功", function () {
		            parent.layoutlist.refresh();
		            _this.cancel()
		        })

		    }
		    else {
		        if (data.Message) {
		            dlg.alert(data.Message)

		        }
		        else {
		            dlg.alert("保存失败")

		        }
		    }
		}
		, "json")

	}
	, cancel: function () {
	    var _this = this;
	    parent.layer.close(_this.options.index)

	}
	, search: function () {
	    var _this = this;
	    _this.grid.refresh()

	}
	, bind: function () {
	    var _this = this;
	    $("#btn-save").on("click", function () {
	        _this.save()

	    });
	    $("#btn-cancel").on("click", function () {
	        _this.cancel()

	    });
	    $("#btn-remove,#btn-delete").on("click", function () {
	        _this.remove()

	    });
	    $("#btn-search").on("click", function () {
	        _this.search()

	    });
	    $("#btn-select").on("click", function () {
	        _this.select()

	    });
	    $("#KeyWord").keydown(function (event) {
	        event = document.all ? window.event : event;
	        if ((event.keyCode || event.which) == 13) {
	            _this.search()

	        }
	    })

	}
	, _rendergrid: function () {
	    var _this = this;
	    _this.grid = new pagegride({
	        id: "dicgrid", url: '/User/RelationAjaxList', columns: [{
	            field: "ID", title: "ID", width: 50

	        }
			, {
			    field: "Name", title: "名称", width: 200

			}
	        ], params: {
	            selectable: "multiple",

	        }
			, searchcontrols: [{
			    field: "KeyWord"

			}
			], pageSize: 50, ischeckbox: true, dbclick: function () {
			    _this.select()

			}
	    })

	}
}