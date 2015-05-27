function toPersianDigits(text) {
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

function GetNowTimeFa() {
    var now = new Date();
    var time = now.getHours() + ':' + now.getMinutes() + ':' + now.getSeconds();
    return toPersianDigits(time);
}

function toByteUnitPersian(num) {
    if (num <= 1024) {
        return 'یک کیلوبایت';
    }
    var value;
    if (num >= 1024 && num < 1024 * 1024) {
        value = num / 1024;
        return Math.round(value) + ' کیلوبایت';
    }
    if (num >= 1024 && num < 1024 * 1024) {
        value = num / (1024 * 1024);
        return Math.round(value) + ' مگابایت';
    }
    if (num >= 1024 * 1024 * 1024) {
        value = num / (1024 * 1024 * 1024);
        return Math.round(value) + ' گیگابایت';
    }
    return '';
}

// SecondryContent >>> start :
function onSecondryPinClick() {
    $('.secondry-content').toggleClass('pin-secondry');
    $('.mainpart-con').toggleClass('pin-secondry');
}

function showSecondryContent() {
    $('.secondry-content').removeClass('tie-secondry');
}

function fillSecondryContent(html) {
    $('#secondry_content').html(html);
}

function hideSecondryContent() {
    $('.secondry-content').addClass('tie-secondry');

    if ($('.mainpart-con').hasClass('pin-secondry')) {
        $('.secondry-content').removeClass('pin-secondry');
        $('.mainpart-con').removeClass('pin-secondry');
    }
}

function hideAndClearSecondryContent() {
    $('.secondry-content').addClass('tie-secondry');

    if ($('.mainpart-con').hasClass('pin-secondry')) {
        $('.secondry-content').removeClass('pin-secondry');
        $('.mainpart-con').removeClass('pin-secondry');
    }

    $('#secondry_content').text('');
}

function checkpointSecondryContent() {
    var content = $('#secondry_content').html();
    $('.secondry-content .previous').html(content);
}

function restoreSecondryContent() {
    var pre = $('.secondry-content .previous').html();
    $('#secondry_content').html(pre);
}
// SecondryContent <<< end.

// GlobalMessage >>> start :
function GlobalMessage(date, message, type) {
    var $messagebox = $('.global-messagebox');
    $messagebox.fadeOut(100);
    setTimeout(function () {
        $messagebox.attr('class', '');
        $messagebox.addClass('global-messagebox').addClass(type);
        $messagebox.find('.date').text(date);
        $messagebox.find('.message').find('span').text(message);
        $messagebox.fadeIn(200);
    }, 100);
}
// GlobalMessage <<< end.

// Popup >>> start :
function showPopupAlert(question, onclickYes, onclickNo) {
    $('.popup-con .alertbox').find('.question').text(question);
    $('.popup-con .alertbox').find('.yes').attr('onclick', 'hidePopupAlert();' + onclickYes);
    $('.popup-con .alertbox').find('.no').attr('onclick', 'hidePopupAlert();' + onclickNo);
    $('.popup-con').addClass('showed');
    $('.popup-con .alertbox').fadeIn(200);
}

function hidePopupAlert() {
    $('.popup-con .alertbox').fadeOut(200);
    setTimeout(function () {
        $('.popup-con').removeClass('showed');
    }, 200);
}

function showPopupBox(id, topic, content) {
    $('.popup-con').addClass('showed');
    $('.popup-con .dynamic-content').find('input#PopBoxId').val(id);
    $('.popup-con .dynamic-content').find('.topic').text(topic);
    $('.popup-con .dynamic-content').find('.content').html(content);
    $('.popup-con .dynamic-content').fadeIn(200);
}

function fullPicturePopup(title, imageSrc) {
    var $image = $('<img />');
    $image.attr('src', imageSrc);

    var $divider = $('<div></div>');
    $divider.addClass('divider-blank30');

    var $button = $('<button></button>');
    $button.text('بستن').addClass('button').attr('onclick', 'cancelPopupBox()');

    var $buttonCon = $('<div></div>');
    $buttonCon.append($button);

    var $imageCon = $('<div></div>');
    $imageCon.addClass('image');
    $imageCon.append($image).append($divider).append($buttonCon);

    showPopupBox('', title, $imageCon);
}

function cancelPopupBox() {
    $('.popup-con .dynamic-content').fadeOut(200);
    setTimeout(function () {
        $('.popup-con').removeClass('showed');
        $('.popup-con .dynamic-content').find('.content').html('');
    }, 200);
}

function loadPopupFileChioce(uniqueId, title, page) {
    $.ajax({
        url: '/Author/Files/Popup',
        data: {
            Page: page,
        },
        type: "POST",
        success: function (r) {
            showPopupBox(uniqueId, title, r);
        }
    });
}

function loadPopupFileChioce_FixedType(uniqueId, title, fileType, page) {
    $.ajax({
        url: '/Author/Files/Popup',
        data: {
            Page: page,
            Type: fileType,
            IsFixedType: true,
        },
        type: "POST",
        success: function (r) {
            showPopupBox(uniqueId, title, r);
        }
    });
}
// Popup <<< end.

// File Box >>> start :
function onfileBoxClick(e) {
    var file = $(e).find('input[type="file"]')[0];
    file.click();
}
// File Box <<< end.

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