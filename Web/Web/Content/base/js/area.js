var area = {
    init: function (option) {
        var _this = this;
        this.option = {};
        $.extend(this.option, option || {});
        _this.option.$sel_province = $("#Sel_" + _this.option.ProvinceFieldName);
        _this.option.$sel_city = $("#Sel_" + _this.option.CityFieldName);
        _this.option.$sel_area = $("#Sel_" + _this.option.AreaFieldName);

        _this.option.$province = $("#" + _this.option.ProvinceFieldName);
        _this.option.$city = $("#" + _this.option.CityFieldName);
        _this.option.$area = $("#" + _this.option.AreaFieldName);

        this.load_province();
    },
    //省
    load_province: function () {
        var _this = this;
        $.getJSON("/Data/GetProvinceList", {}, function (data) {
            $.each(data, function (m, n) {
                _this.option.$sel_province.append('<option value="' + n.ID + '" ' + (n.ID == _this.option.$province.val() ? "selected=\"selected\"" : "") + '>' + n.Name + '</option>');
            });
            _this.load_city($(_this.option.$province).val());
        });
        _this.option.$sel_province.change(function () {
            _this.option.$province.val($(this).val());
            _this.option.$city.val(""); _this.option.$sel_city.find("option:gt(0)").remove();
            _this.option.$area.val(""); _this.option.$sel_area.find("option:gt(0)").remove();
            _this.load_city($(this).val());
        });
    },
    //市
    load_city: function (provinceid) {
        var _this = this;
        $.getJSON("/Data/GetCityListByProvinceID", { id: provinceid }, function (data) {
            _this.option.$sel_city.find("option:gt(0)").remove();
            $.each(data, function (m, n) {
                _this.option.$sel_city.append('<option value="' + n.ID + '" ' + (n.ID == _this.option.$city.val() ? "selected=\"selected\"" : "") + '>' + n.Name + '</option>');
            });
            _this.load_area(_this.option.$city.val());
        });
        _this.option.$sel_city.change(function () {
            _this.option.$city.val($(this).val());
            _this.load_area($(this).val());
        });
    },
    //区
    load_area: function (cityid) {
        var _this = this;
        $.getJSON("/Data/GetAreaListByCityID", { id: cityid }, function (data) {
            _this.option.$sel_area.find("option:gt(0)").remove();
            $.each(data, function (m, n) {
                _this.option.$sel_area.append('<option value="' + n.ID + '" ' + (n.ID == _this.option.$area.val() ? "selected=\"selected\"" : "") + '>' + n.Name + '</option>');
            });
        });
        _this.option.$sel_area.change(function () {
            _this.option.$area.val($(this).val());
        });
    }
};