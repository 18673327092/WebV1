/***************************************************************** 
                  jQuery Validate扩展验证方法        
*****************************************************************/  
  
// 判断数值类型，包括整数和浮点数  
jQuery.validator.addMethod("iszhekou", function (value, element) {
    return this.optional(element) || /^[0]{0,1}(\.\d{0,2})?$/.test(value);
}, "请输入正确格式的折扣,8.5折对应0.85 ");

// 判断浮点数value是否等于0   
jQuery.validator.addMethod("isfloateqzero", function (value, element) {  
    value = parseFloat(value);  
    return this.optional(element) || value == 0;  
}, "浮点数必须为0");  

 
// 判断浮点数value是否大于0  
jQuery.validator.addMethod("isfloatgtzero", function (value, element) {
    value = parseFloat(value);  
    return this.optional(element) || value > 0;  
}, "浮点数必须大于0");  
  
  
// 判断浮点数value是否大于或等于0  
jQuery.validator.addMethod("isfloatgtezero", function (value, element) {  
    value = parseFloat(value);  
    return this.optional(element) || value >= 0;  
}, "浮点数必须大于或等于0");  
  
  
// 判断浮点数value是否不等于0   
jQuery.validator.addMethod("isfloatneqzero", function (value, element) {  
    value = parseFloat(value);  
    return this.optional(element) || value != 0;  
}, "浮点数必须不等于0");  
  
  
// 判断浮点数value是否小于0   
jQuery.validator.addMethod("isfloatltzero", function (value, element) {  
    value = parseFloat(value);  
    return this.optional(element) || value < 0;  
}, "浮点数必须小于0");  
  
  
// 判断浮点数value是否小于或等于0   
jQuery.validator.addMethod("isfloatltezero", function (value, element) {  
    value = parseFloat(value);  
    return this.optional(element) || value <= 0;  
}, "浮点数必须小于或等于0");  
  
  
// 判断浮点型    
jQuery.validator.addMethod("isfloat", function (value, element) {  
    return this.optional(element) || /^[-\+]?\d+(\.\d+)?$/.test(value);  
}, "只能包含数字、小数点等字符");  
  
  
// 匹配integer  
jQuery.validator.addMethod("isinteger", function (value, element) {  
    return this.optional(element) || (/^[-\+]?\d+$/.test(value) && parseInt(value) >= 0);  
}, "只能输入整数");  
  
  
// 判断数值类型，包括整数和浮点数  
jQuery.validator.addMethod("isnumber", function (value, element) {  
    return this.optional(element) || /^[-\+]?\d+$/.test(value) || /^[-\+]?\d+(\.\d+)?$/.test(value);  
}, "匹配数值类型，包括整数和浮点数");  
  
  
// 只能输入[0-9]数字  
jQuery.validator.addMethod("isdigits", function (value, element) {  
    return this.optional(element) || /^\d+$/.test(value);  
}, "只能输入0-9数字");  
  
  
// 判断中文字符   
jQuery.validator.addMethod("ischinese", function (value, element) {  
    return this.optional(element) || /^[\u0391-\uFFE5]+$/.test(value);  
}, "只能包含中文字符。");  
  
  
// 判断英文字符   
jQuery.validator.addMethod("isenglish", function (value, element) {  
    return this.optional(element) || /^[A-Za-z]+$/.test(value);  
}, "只能包含英文字符。");  
  
// 判断日期类型    
jQuery.validator.addMethod("isdate", function (value, element) {
    return this.optional(element) || /^\d{4}-\d{2}-\d{2}$/.test(value);
}, "请输入有效的日期");


// 判断时间类型    
jQuery.validator.addMethod("istime", function (value, element) {
    return this.optional(element) || /^[1-9]\d{3}-(0[1-9]|1[0-2])-(0[1-9]|[1-2][0-9]|3[0-1])\s+(20|21|22|23|[0-1]\d):[0-5]\d:[0-5]\d$/.test(value);
}, "请输入有效的时间");
  
// 手机号码验证      
jQuery.validator.addMethod("ismobile", function (value, element) {  
    var length = value.length;  
    return this.optional(element) || (length == 11 && /^(((13[0-9]{1})|(15[0-9]{1})|(18[0-9]{1}))+\d{8})$/.test(value));  
}, "请正确填写您的手机号码。");  
  
  
// 电话号码验证      
jQuery.validator.addMethod("isphone", function (value, element) {  
    var tel = /^(\d{3,4}-?)?\d{7,9}$/g;  
    return this.optional(element) || (tel.test(value));  
}, "请正确填写您的电话号码。");  
  
  
// 联系电话(手机/电话皆可)验证     
jQuery.validator.addMethod("istel", function (value, element) {  
    var length = value.length;  
    var mobile = /^(((13[0-9]{1})|(15[0-9]{1})|(18[0-9]{1}))+\d{8})$/;  
    var tel = /^(\d{3,4}-?)?\d{7,9}$/g;  
    return this.optional(element) || tel.test(value) || (length == 11 && mobile.test(value));  
}, "请正确填写您的联系方式");  
  
  
// 匹配qq        
jQuery.validator.addMethod("isqq", function (value, element) {  
    return this.optional(element) || /^[1-9]\d{4,12}$/;  
}, "匹配QQ");  
  
  
// 邮政编码验证      
jQuery.validator.addMethod("isZipCode", function (value, element) {  
    var zip = /^[0-9]{6}$/;  
    return this.optional(element) || (zip.test(value));  
}, "请正确填写您的邮政编码。");  
  
  
// 匹配密码，以字母开头，长度在6-12之间，只能包含字符、数字和下划线。        
jQuery.validator.addMethod("ispwd", function (value, element) {  
    return this.optional(element) || /^[a-zA-Z]\\w{6,12}$/.test(value);  
}, "以字母开头，长度在6-12之间，只能包含字符、数字和下划线。");  
  
  
// 身份证号码验证  
jQuery.validator.addMethod("isidcardno", function (value, element) {  
    //var idCard = /^(\d{6})()?(\d{4})(\d{2})(\d{2})(\d{3})(\w)$/;     
    return this.optional(element) || isIdCardNo(value);  
}, "请输入正确的身份证号码。");  
  
  
// IP地址验证     
jQuery.validator.addMethod("ip", function (value, element) {  
    return this.optional(element) || /^(([1-9]|([1-9]\d)|(1\d\d)|(2([0-4]\d|5[0-5])))\.)(([1-9]|([1-9]\d)|(1\d\d)|(2([0-4]\d|5[0-5])))\.){2}([1-9]|([1-9]\d)|(1\d\d)|(2([0-4]\d|5[0-5])))$/.test(value);  
}, "请填写正确的IP地址。");  
  
  
// 字符验证，只能包含中文、英文、数字、下划线等字符。      
jQuery.validator.addMethod("stringcheck", function (value, element) {  
    return this.optional(element) || /^[a-zA-Z0-9\u4e00-\u9fa5-_]+$/.test(value);  
}, "只能包含中文、英文、数字、下划线等字符");  
  
// 字符验证，只能包含中文、英文、数字、下划线等字符。      
jQuery.validator.addMethod("none", function (value, element) {
    return true;
}, "");
  
// 匹配english    
jQuery.validator.addMethod("isenglish", function (value, element) {  
    return this.optional(element) || /^[A-Za-z]+$/.test(value);  
}, "匹配english");  
  
// 匹配中文(包括汉字和字符)   
jQuery.validator.addMethod("ischinesechar", function (value, element) {  
    return this.optional(element) || /^[\u0391-\uFFE5]+$/.test(value);  
}, "匹配中文(包括汉字和字符) ");  
  
  
// 判断是否为合法字符(a-zA-Z0-9-_)  
jQuery.validator.addMethod("isrightfulatring", function (value, element) {  
    return this.optional(element) || /^[A-Za-z0-9_-]+$/.test(value);  
}, "判断是否为合法字符(a-zA-Z0-9-_)");  
  
  
  
// 判断是否金额 小数点后两位  
jQuery.validator.addMethod("isamount", function (value, element) {
    return this.optional(element) || /^0\.([0-9]|\d[0-9])$|^[0-9]\d{0,8}\.\d{0,2}$|^[1-9]\d{0,8}$/.test(value);
},"金额必须大于等于0且小数位数不超过2位");  
  
//身份证号码的验证规则  
function isIdCardNo(num) {  
    //if (isNaN(num)) {alert("输入的不是数字！"); return false;}   
    var len = num.length, re;  
    if (len == 15)  
        re = new RegExp(/^(\d{6})()?(\d{2})(\d{2})(\d{2})(\d{2})(\w)$/);  
    else if (len == 18)  
        re = new RegExp(/^(\d{6})()?(\d{4})(\d{2})(\d{2})(\d{3})(\w)$/);  
    else {  
        //alert("输入的数字位数不对。");   
        return false;  
    }  
    var a = num.match(re);  
    if (a != null) {  
        if (len == 15) {  
            var D = new Date("19" + a[3] + "/" + a[4] + "/" + a[5]);  
            var B = D.getYear() == a[3] && (D.getMonth() + 1) == a[4] && D.getDate() == a[5];  
        }  
        else {  
            var D = new Date(a[3] + "/" + a[4] + "/" + a[5]);  
            var B = D.getFullYear() == a[3] && (D.getMonth() + 1) == a[4] && D.getDate() == a[5];  
        }  
        if (!B) {  
            //alert("输入的身份证号 "+ a[0] +" 里出生日期不对。");   
            return false;  
        }  
    }  
    if (!re.test(num)) {  
        //alert("身份证最后一位只能是数字和字母。");  
        return false;  
    }  
    return true;  
}  