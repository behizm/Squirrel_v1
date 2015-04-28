﻿function toPersianDigits(text) {
    /* ۰ ۱ ۲ ۳ ۴ ۵ ۶ ۷ ۸ ۹ */
    if (text == undefined) return '';
    var str1 = $.trim(text.toString());
    if (str1 == '') return '';
    str1 = str1.replace(/0/g, '۰');
    str1 = str1.replace(/1/g, '۱');
    str1 = str1.replace(/2/g, '۲');
    str1 = str1.replace(/3/g, '۳');
    str1 = str1.replace(/4/g, '۴');
    str1 = str1.replace(/5/g, '۵');
    str1 = str1.replace(/6/g, '۶');
    str1 = str1.replace(/7/g, '۷');
    str1 = str1.replace(/8/g, '۸');
    str1 = str1.replace(/9/g, '۹');
    return str1;
}

function toEnglishDigits(text) {
    if (text == undefined) return '';
    var str = $.trim(text.toString());
    if (str == "") return "";
    str = str.replace(/۰/g, '0');
    str = str.replace(/۱/g, '1');
    str = str.replace(/۲/g, '2');
    str = str.replace(/۳/g, '3');
    str = str.replace(/۴/g, '4');
    str = str.replace(/۵/g, '5');
    str = str.replace(/۶/g, '6');
    str = str.replace(/۷/g, '7');
    str = str.replace(/۸/g, '8');
    str = str.replace(/۹/g, '9');
    return str;
}

(function ($) {

    $(function () {
        // Material Textbox >>> start :
        $('.materialtext .textbox, .materialtext .textarea').each(function (i) {
            if ($(this).val() != '') {
                $(this).parent().addClass('value');
            }
        });

        $('.materialtext .textbox, .materialtext .textarea').focusin(function () {
            $(this).parent().addClass('focus');
            $(this).parent().addClass('value');
        });
        $('.materialtext .textbox, .materialtext .textarea').focusout(function () {
            if ($(this).val() == '') {
                $(this).parent().removeClass('value');
            }
            $(this).parent().removeClass('focus');
        });
        // Material Textbox <<< end.
    });


    //DropDownList
    $.fn.CustomizeDropDownList = function (className) {
        this.wrap('<div class="dropdownlist ' + className + '"></div>');
        var ddl = this.parent('.dropdownlist');
        var selectText = this.find(':selected').text();
        ddl.append('<div class="selecttext"></div>');
        ddl.children('.selecttext').text(selectText);
        ddl.append('<ul></ul>');
        this.children('option').each(function (x) {
            var thisText = ddl.children('select').children('option:eq(' + x + ')').text();
            if (thisText == selectText) {
                ddl.children('ul').append('<li class="selected">' + thisText + '</li>');
            }
            else {
                ddl.children('ul').append('<li>' + thisText + '</li>');
            }
        });
    };

})($)