var error_coount = 0;
var oldfocus;
var signaturePad = {};
var proxy_a = new URL(document.querySelector('meta[name=homepage]').content);
var glightbox;
var _dateLocal = typeof(_datePickerLocal) !== 'undefined' ? _datePickerLocal : 'th';
var datatable_language_default = {
    "sEmptyTable": "ไม่มีข้อมูลในตาราง",
    "sInfo": "แสดง _START_ ถึง _END_ จาก _TOTAL_ แถว",
    "sInfoEmpty": "แสดง 0 ถึง 0 จาก 0 แถว",
    "sInfoFiltered": "(กรองข้อมูล _MAX_ ทุกแถว)",
    "sInfoPostFix": "",
    "sInfoThousands": ",",
    "sLengthMenu": "แสดง _MENU_ แถว",
    "sLoadingRecords": "กำลังโหลดข้อมูล...",
    "sProcessing": "กำลังดำเนินการ...",
    "sSearch": "ค้นหา: ",
    "sZeroRecords": "ไม่พบข้อมูล",
    "oPaginate": {
        "sFirst": "หน้าแรก",
        "sPrevious": "ก่อนหน้า",
        "sNext": "ถัดไป",
        "sLast": "หน้าสุดท้าย"
    },
    "oAria": {
        "sSortAscending": ": เปิดใช้งานการเรียงข้อมูลจากน้อยไปมาก",
        "sSortDescending": ": เปิดใช้งานการเรียงข้อมูลจากมากไปน้อย"
    }
}

var genBtnClearAndToDay = function (id){
    return `
        <div class="d-flex">
        <button class="btn btn-default btn-xs w-100" onclick="$('#${id}').val('').trigger('change')"> Clear </buttom>
        <button class="btn btn-default btn-xs w-100" onclick="$('#${id}').val(moment().format('YYYY-MM-DD hh:mm')).trigger('change')"> ToDay </buttom>
        </div>
    `;
}

var _datatable_language = typeof(datatable_language) !== 'undefined' ? datatable_language : datatable_language_default;
// window.CKEDITOR_BASEPATH = document.querySelector('meta[name=homepage]').getAttribute("content").replace('/home', '') + '/js/ckeditor/';

let dateLookupthai = {
    "daysOfWeek": ["อา.","จ.","อ.","พ.","พฤ","ศ.","ส."],
    "monthNames": [
        "ม.ค.",
        "ก.พ.",
        "มี.ค.",
        "เม.ย.",
        "พ.ค.",
        "มิ.ย.",
        "ก.ค.",
        "ส.ค.",
        "ก.ย.",
        "ต.ค.",
        "พ.ย.",
        "ธ.ค.",
    ]
}

var floatTheadReset = _.debounce(function () {
    $('.float-thead').floatThead('destroy');
    setTimeout(() => {
        $('.float-thead').each((index, el) => {
            var tbl = $(el);
            tbl.floatThead({
                responsiveContainer: function(tbl) {
                    return tbl.closest('.table-responsive');
                },
                zIndex: 50,
                scrollContainer: function($table) {
                    return $table.closest('.table-scroll-container');
                },
                top: $('.content-header').outerHeight() || 0
            });
        });
    }, 300);
},100);

var _helper = {
    init() {

        $('.datepicker').each((index, item)=>{
            flatpickr(item, {
                locale: _dateLocal,
                minDate:item.dataset.minDate ?? '',
                maxDate:item.dataset.maxDate ?? '',
                enableTime: false,
                dateFormat: "Y-m-d",
                altInput: true, 
                altFormat: "d-m-Y", 
                onReady: function(selectedDates, dateStr, instance){
                    // $(`.flatpickr-calendar.init-${instance.input.id}`).remove();
                    // $(instance.calendarContainer).addClass(`init-${instance.input.id}`);
                    $(instance.calendarContainer).append(genBtnClearAndToDay(instance.input.id));
                }
            });
        })
        // # datetimepicker
        $('.datetimepicker').each((index, item)=>{
            flatpickr(item, {
                locale: _dateLocal,
                minDate:item.dataset.minDate ?? '',
                maxDate:item.dataset.maxDate ?? '',
                enableTime: true,
                dateFormat: "Y-m-d H:i",
                altInput: true, 
                altFormat: "d-m-Y H:i", 
                time_24hr: true,
                onReady: function(selectedDates, dateStr, instance){
                    // $(`.flatpickr-calendar.init-${instance.input.id}`).remove();
                    // $(instance.calendarContainer).addClass(`init-${instance.input.id}`);
                    $(instance.calendarContainer).append(genBtnClearAndToDay(instance.input.id));
                }
            });
        })
        // # daterangepicker
        $('.daterangepicker').each((index, item)=>{
            flatpickr(item, {
                locale: _dateLocal,
                mode: "range",
                minDate:item.dataset.minDate ?? '',
                maxDate:item.dataset.maxDate ?? '',
                enableTime: false,
                dateFormat: "Y-m-d",
                altInput: true, 
                altFormat: "d-m-Y", 
                time_24hr: true,
                onReady: function(selectedDates, dateStr, instance){
                    // $(`.flatpickr-calendar.init-${instance.input.id}`).remove();
                    // $(instance.calendarContainer).addClass(`init-${instance.input.id}`);
                    $(instance.calendarContainer).append(genBtnClearAndToDay(instance.input.id));
                }

            });
        })
        // # timepicker
        $('.timepicker').each((index, item)=>{
            flatpickr(item, {
                locale: _dateLocal,
                enableTime: true,
                noCalendar: true,
                dateFormat: "H:i",
                minTime: item.dataset.minTime,
                maxTime: item.dataset.maxTime,
                time_24hr: true,
                onReady: function(selectedDates, dateStr, instance){
                    // $(`.flatpickr-calendar.init-${instance.input.id}`).remove();
                    // $(instance.calendarContainer).addClass(`init-${instance.input.id}`);
                    $(instance.calendarContainer).append(genBtnClearAndToDay(instance.input.id));
                }
            });
        })

        // # monthpicker
        $('.monthpicker').each((index, item)=>{
            flatpickr(item, {
                locale: _dateLocal,
                plugins: [
                    new monthSelectPlugin({
                      shorthand: true, //defaults to false
                      dateFormat: "Y-m-01", //defaults to "F Y"
                      altFormat: "F Y", //defaults to "F Y"
                      theme: "light" // defaults to "light"
                    })
                ],
                onReady: function(selectedDates, dateStr, instance){
                    // $(`.flatpickr-calendar.init-${instance.input.id}`).remove();
                    // $(instance.calendarContainer).addClass(`init-${instance.input.id}`);
                    $(instance.calendarContainer).append(genBtnClearAndToDay(instance.input.id));
                }
            });
        })

        $(document).on('change','.datepicker,.datetimepicker,.monthpicker', (e)=>{ 
            try {
                let newdate = "";
                let thisvalue = e.target.value;
                if(thisvalue){
                    if(thisvalue.indexOf('-') == 4 || thisvalue.indexOf('/') == 4){
                        newdate = moment(thisvalue).format();
                    }else if (thisvalue.indexOf('/') > -1) {
                        newdate = moment(thisvalue,"DD/MM/YYYY hh:mm").format();
                    }else{
                        newdate = moment(thisvalue,"DD-MM-YYYY hh:mm").format()
                    }
                    e.target._flatpickr.setDate(new Date(newdate))
                }else{
                    e.target._flatpickr.setDate(thisvalue)
                }
            } catch (error) {
            }            
        });

        floatTheadReset()

        //Initialize Select2 Elements

        $('.table-responsive').on('show.bs.dropdown', function() {
            // console.log($('.table-responsive').scrollLeft());
            $('.table-responsive').css("overflow", "inherit");
        });

        $('.table-responsive').on('hide.bs.dropdown', function() {
            $('.table-responsive').css("overflow", "auto");
        });

        $(function() {
            $('[data-toggle="tooltip"]').tooltip();
            $('.wrapper').attr('style', 'height: -webkit-fill-available;height: auto;min-height: 100%;');
        })

        $('.select2').select2();
        $('.autocomplete').autocomplete();
        $('[data-mask]').inputmask();

        $('.form-loading').on('submit', function(el) {
            try { if(!$(el.target).valid()) return false; } catch (error) { }            
            if($(el.target).has('.modal').length){
                _helper.loading($(el.target).find('.modal'), 10000);
                $(el.target).find('.ajax-loading').eq(0).css('position', 'fixed');
            }else{
                _helper.loading(el.target, 10000);
                $(el.target).find('.ajax-loading').eq(0).css('position', 'fixed');
            }
        });

        $('[can]').checkPermission();
        // $('#ajax-first-load').remove();
        $('#ajax-first-load').fadeOut('fast');
        setTimeout(() => {
            $('#ajax-first-load').remove();
        }, 500);

        tooltipInit();
        inputFilterInit();
        signaturePadInit();
        setColorZero();
        glightbox = GLightbox();

    },
    updateInint(el) {
        el = el || 'body';
        $(el + ' .datepicker').each((index, item)=>{
            //flatpickr(item).destroy();
            flatpickr(item, {
                locale: _dateLocal,
                minDate:item.dataset.minDate ?? '',
                maxDate:item.dataset.maxDate ?? '',
                enableTime: false,
                dateFormat: "Y-m-d",
                altInput: true, 
                altFormat: "d-m-Y",
                onReady: function(selectedDates, dateStr, instance){
                    $(`.flatpickr-calendar.init-${instance.input.id}`).remove();
                    $(instance.calendarContainer).addClass(`init-${instance.input.id}`);
                    $(instance.calendarContainer).append(genBtnClearAndToDay(instance.input.id));
                }
            });
        })
        // # datetimepicker
        $(el + ' .datetimepicker').each((index, item)=>{
            //flatpickr(item).destroy();
            flatpickr(item, {
                locale: _dateLocal,
                minDate:item.dataset.minDate ?? '',
                maxDate:item.dataset.maxDate ?? '',
                enableTime: true,
                dateFormat: "Y-m-d H:i",
                altInput: true, 
                altFormat: "d-m-Y H:i", 
                time_24hr: true,
                onReady: function(selectedDates, dateStr, instance){
                    $(`.flatpickr-calendar.init-${instance.input.id}`).remove();
                    $(instance.calendarContainer).addClass(`init-${instance.input.id}`);
                    $(instance.calendarContainer).append(genBtnClearAndToDay(instance.input.id));
                }
            });
        })
        // # daterangepicker
        $(el + ' .daterangepicker').each((index, item)=>{
            //flatpickr(item).destroy();
            flatpickr(item, {
                locale: _dateLocal,
                mode: "range",
                minDate:item.dataset.minDate ?? '',
                maxDate:item.dataset.maxDate ?? '',
                enableTime: false,
                dateFormat: "Y-m-d",
                altInput: true, 
                altFormat: "d-m-Y", 
                time_24hr: true,
                onReady: function(selectedDates, dateStr, instance){
                    $(`.flatpickr-calendar.init-${instance.input.id}`).remove();
                    $(instance.calendarContainer).addClass(`init-${instance.input.id}`);
                    $(instance.calendarContainer).append(genBtnClearAndToDay(instance.input.id));
                }

            });
        })
        // # timepicker
        $(el + ' .timepicker').each((index, item)=>{
            //flatpickr(item).destroy();
            flatpickr(item, {
                locale: _dateLocal,
                enableTime: true,
                noCalendar: true,
                dateFormat: "H:i",
                minTime: item.dataset.minTime,
                maxTime: item.dataset.maxTime,
                time_24hr: true,
                onReady: function(selectedDates, dateStr, instance){
                    $(`.flatpickr-calendar.init-${instance.input.id}`).remove();
                    $(instance.calendarContainer).addClass(`init-${instance.input.id}`);
                    $(instance.calendarContainer).append(genBtnClearAndToDay(instance.input.id));
                }
            });
        })

        // # monthpicker
        $(el + ' .monthpicker').each((index, item)=>{
            //flatpickr(item).destroy();
            flatpickr(item, {
                locale: _dateLocal,
                plugins: [
                    new monthSelectPlugin({
                      shorthand: true, //defaults to false
                      dateFormat: "yyyy-mm-01", //defaults to "F Y"
                      altFormat: "F Y", //defaults to "F Y"
                      theme: "light" // defaults to "light"
                    })
                ],
                onReady: function(selectedDates, dateStr, instance){
                    $(`.flatpickr-calendar.init-${instance.input.id}`).remove();
                    $(instance.calendarContainer).addClass(`init-${instance.input.id}`);
                    $(instance.calendarContainer).append(genBtnClearAndToDay(instance.input.id));
                }
            });
        })

        $(el + ' .datatable-full').each((index, el) => {
            let tableable = $(el).find('thead').eq(0).find('th').map((index2, el2) => {
                if ($(el2).attr('orderable') == "false") {
                    return index2;
                }
            });
            tableable = [...tableable];
            let pageLength = $(el).attr('pageLength') ? parseInt($(el).attr('pageLength')) : 10;
            let lengthMenu = [
                [..._.uniq([pageLength, 10, 25, 30, 50]).sort(), -1],
                [..._.uniq([pageLength, 10, 25, 30, 50]).sort(), "All"]
            ]
            $(el).DataTable({
                'pageLength': pageLength,
                'lengthMenu': lengthMenu,
                'columnDefs': [{
                    'targets': tableable,
                    'orderable': false,
                }],
                "language": _datatable_language
            });
        });

        $(el + ' .datatable').each((index, el) => {
            let tableable = $(el).find('thead').eq(0).find('th').map((index2, el2) => {
                if ($(el2).attr('orderable') == "false") {
                    return index2;
                }
            });
            tableable = [...tableable];
            let pageLength = $(el).attr('pageLength') ? parseInt($(el).attr('pageLength')) : 10;
            let lengthMenu = [
                [..._.uniq([pageLength, 10, 25, 30, 50]).sort(), -1],
                [..._.uniq([pageLength, 10, 25, 30, 50]).sort(), "All"]
            ]
            $(el).DataTable({
                'pageLength': pageLength,
                'lengthMenu': lengthMenu,
                'paging': true,
                'lengthChange': false,
                'searching': false,
                'ordering': true,
                'info': true,
                'autoWidth': false,
                'columnDefs': [{
                    'targets': tableable,
                    'orderable': false,
                }],
                "language": _datatable_language
            });
        });

        $(el + ' .float-thead').floatThead('destroy');
        $(el + ' .float-thead').each((index, el) => {

            var tbl = $(el);
            tbl.floatThead({
                responsiveContainer: function(tbl) {
                    return tbl.closest('.table-responsive');
                },
                zIndex: 50,
                scrollContainer: function($table) {
                    return $table.closest('.table-scroll-container');
                },
                top: $('.content-header').outerHeight() || 0
            });

        });

        $(el + ' .select2').select2()
        $(el + ' .autocomplete').autocomplete();
        $(el + ' [data-mask]').inputmask()

        $(el + ' .form-loading').on('submit', function(el) {
            try { if(!$(el.target).valid()) return false; } catch (error) { }
            _helper.loading(el.target, 10000);
            $(el.target).find('.ajax-loading').eq(0).css('position', 'fixed');
        });

        $('[can]').checkPermission();

        tooltipInit();
        inputFilterInit();
        signaturePadInit();
        setColorZero();

        //reload validate
        $.validator.unobtrusive.parse(el);
    },
    startLoading(el) {
        $(document).ready(function() {
            $(el).find('.ajax-loading').remove();
            $(el).prepend(` <div class="ajax-loading"></div> `);
            oldfocus = $(el).find('*:focus');
            oldfocus.blur();
        })
    },
    loading(el) {
        $(el).find('.ajax-loading').remove();
        $(el).prepend(` <div class="ajax-loading"></div> `);
        oldfocus = $(el).find('*:focus');
        oldfocus.blur();
    },
    stopLoading(el) {
        $(document).ready(function() {
            setTimeout(() => {
                if(el == "section#main-content.content"){
                    $(el).children('.ajax-loading').eq(0).remove();
                }else{
                    $(el).find('.ajax-loading').remove();
                }
                try { oldfocus.focus(); } catch (error) {}
            }, 200);
        })
    },
    numberFormat(num, digis) {
        num = Number(num) == NaN ? 0 : Number(num);
        if (digis === undefined || Number(digis) === NaN) {
            return num.toFixed(2).replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,')
        } else if (digis < 0) {
            return num.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,')
        }
        return num.toFixed(digis).replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,')
    },

    popup(url, name, _w, _h) {
        var w = _w ? _w : 1200;
        var h = _h ? _h : 500;
        var y = window.outerHeight / 2 + window.screenY - (h / 2);
        var x = window.outerWidth / 2 + window.screenX - (w / 2);
        var _name = name ? name : "myWindow"
        myWindow = window.open(url, _name, "width=" + w + ",height=" + h + ", top=" + y + ", left=" + x); // Opens a new window
    },

    popupPost(actionUrl, windowName, windowFeatures, keyParams, valueParams, _w, _h) {
        var w = _w ? _w : 1200;
        var h = _h ? _h : 500;
        var y = window.outerHeight / 2 + window.screenY - (h / 2)
        var x = window.outerWidth / 2 + window.screenX - (w / 2)
        windowFeatures = windowFeatures ? windowFeatures : "width=" + w + ",height=" + h + ", top=" + y + ", left=" + x;
        var mapForm = document.createElement("form");
        var milliseconds = new Date().getTime();
        windowName = windowName + milliseconds;
        mapForm.target = windowName;
        mapForm.method = "POST";
        mapForm.action = actionUrl;
        if (keyParams && valueParams && (keyParams.length == valueParams.length)) {
            for (var i = 0; i < keyParams.length; i++) {
                var mapInput = document.createElement("input");
                mapInput.type = "hidden";
                mapInput.name = keyParams[i];
                mapInput.value = valueParams[i];
                mapForm.appendChild(mapInput);

            }
            let tokenInput = document.createElement("input");
            tokenInput.type = "hidden";
            tokenInput.name = '__RequestVerificationToken';
            tokenInput.value = $('input[name=__RequestVerificationToken]').first().val();
            mapForm.appendChild(tokenInput);

            document.body.appendChild(mapForm);
        }
        map = window.open('', windowName, windowFeatures);
        if (map) {
            mapForm.submit();
        } else {
            alert('กรุณา เปิด allow popups ที่ Browser ของท่าน.');
        }
    },

    saveAsFile(buffer, fileName, fileType) {
        if(buffer){
            fileName = fileName ? fileName : Date.now();
            const data = new Blob([buffer], { type: fileType });
            saveAs(data, fileName);
        }else{
            alertBox2('danger', 'buffer not data!');
        }
    },

    exportJsonToCsv(dataJson, fileName, columnsFilter) {
        if (!dataJson || !dataJson.length) {
          return;
        }
        fileName = fileName ? fileName : Date.now();
        const separator = ',';
        const keys = Object.keys(dataJson[0]).filter(k => {
          if (columnsFilter?.length) {
            return columnsFilter.includes(k);
          } else {
            return true;
          }
        });
        const csvContent =
          keys.join(separator) +
          '\n' +
          dataJson.map(row => {
            return keys.map(k => {
              let cell = row[k] === null || row[k] === undefined ? '' : row[k];
              cell = cell instanceof Date
                ? cell.toLocaleString()
                : cell.toString().replace(/"/g, '""');
              if (cell.search(/("|,|\n)/g) >= 0) {
                cell = `"${cell}"`;
              }
              return cell;
            }).join(separator);
          }).join('\n');
        this.saveAsFile(csvContent, `${fileName}.csv`, 'text/plain;charset=utf-8');
    },

    exportJsonToExcel(dataJson, fileName,extensiontype) {
        if(!Array.isArray(dataJson)){
            alertBox2('error','dataJson require format JSON array!');
            return false;
        }
        if(typeof(XLSX) !== 'undefined'){
            fileName = fileName ? fileName : Date.now();
            extensiontype = extensiontype ? extensiontype : '.xlsx';
            const ws = XLSX.utils.json_to_sheet(dataJson);
            // generate workbook and add the worksheet
            const workbook = XLSX.utils.book_new();
            XLSX.utils.book_append_sheet(workbook, ws, 'Sheet1');
            // save to file
            XLSX.writeFile(workbook, `${fileName}${extensiontype}`);
        }
    },

    exportJsonToMultiSheetExcel(dataJson, fileName,extensiontype) {
        //simple dataJson 
        /*
        dataJson = {
            sheet1: [{ "col1":"datacol1","col2": "datacol1" },{ "col1":"datacol2","col2": "datacol2" },{ "col1":"datacol3","col2": "datacol3" }],
            sheet2: [{ "col1":"datacol1","col2": "datacol1" },{ "col1":"datacol2","col2": "datacol2" },{ "col1":"datacol3","col2": "datacol3" }],
            sheet3: [{ "col1":"datacol1","col2": "datacol1" },{ "col1":"datacol2","col2": "datacol2" },{ "col1":"datacol3","col2": "datacol3" }]
            
        }
        */
        if(typeof(dataJson) !== 'object'){
            Swal.fire({
                icon: 'error',
                title: 'dataJson require format',
                html: `
                <pre class="text-left">
                <br>dataJson = {
                <br>sheet1: [{ "col1":"datacol1","col2": "datacol1" },{ "col1":"datacol2","col2": "datacol2" },...],
                <br>sheet2: [{ "col1":"datacol1","col2": "datacol1" },{ "col1":"datacol2","col2": "datacol2" },...],
                <br>sheet3: [{ "col1":"datacol1","col2": "datacol1" },{ "col1":"datacol2","col2": "datacol2" },...]        
                <br>}
                </pre>`
            });
            return false;
        }
        if(typeof(XLSX) !== 'undefined' && typeof(dataJson) == 'object'){
            fileName = fileName ? fileName : Date.now();
            extensiontype = extensiontype ? extensiontype : '.xlsx';
            const workbook = XLSX.utils.book_new();
            Object.keys(dataJson).forEach(sheet=>{
                let ws = XLSX.utils.json_to_sheet(dataJson[sheet]);
                XLSX.utils.book_append_sheet(workbook, ws, sheet);
            })
            // save to file
            XLSX.writeFile(workbook, `${fileName}${extensiontype}`);
        }
    }

};


$('body').on('hidden.bs.modal', function() {
    if ($('.modal.in').length > 0) {
        $('body').addClass('modal-open');
        $('.modal.in').find('input, textarea, select').filter(function(index, el) {
            if (!$(el).prop('readonly') && !$(el).prop('disabled') && $(el).attr('type') != 'hidden') {
                return true;
            }
            return false;
        }).eq(0).focus();
    }
});

$('body').on('shown.bs.modal', function(e) {
    $(e.target).find('input, textarea, select').filter(function(index, el) {
        if (!$(el).prop('readonly') && !$(el).prop('disabled') && $(el).attr('type') != 'hidden' && !$(el).attr('hidden')) {
            return true;
        }
        return false;
    }).eq(0).focus();
});

$(document).on('select2:open', () => {
    document.querySelector('.select2-search__field').focus();
});

// toastr sweetalert2
var Toast = Swal.mixin({
    toast: true,
    position: 'top-end',
    showConfirmButton: false,
    timer: 3000
  });

function alertBox(status, message) {
    let type = 'question';
    if (status == 'success') {
        type = 'success';
        icon = "bi bi-check2-circle";
    } else if (status == 'info') {
        type = 'info';
        icon = "bi bi-info-circle";
    } else if (status == 'warning') {
        type = 'warning';
        icon = "bi bi-exclamation-diamond";
    } else if (status == 'danger') {
        type = 'error';
        icon = "bi bi-exclamation-octagon-fill";
    } else if (status == 'error') {
        type = 'error';
        icon = "bi bi-exclamation-octagon-fill";
    }

    Toast.fire({
        icon: type,
        title: message
    })
}

// toastr notify
function alertBox2(status, message, title, subtitle, delay) {
    let type = 'info';
    let icon = 'bi bi-info-circle';
    if (status == 'success') {
        title = title ? title : "ทำรายการสำเร็จ";
        type = 'success';
        icon = "bi bi-check2-circle";
    } else if (status == 'info') {
        title = title ? title : "แนะนำ";
        type = 'info';
        icon = "bi bi-info-circle";
    } else if (status == 'warning') {
        title = title ? title : "แจ้งเตือน";
        type = 'warning';
        icon = "bi bi-exclamation-diamond";
    } else if (status == 'danger') {
        title = title ? title : "เกิดข้อผิดพลาด";
        type = 'error';
        icon = "bi bi-exclamation-octagon-fill";
    } else if (status == 'error') {
        title = title ? title : "เกิดข้อผิดพลาด";
        type = 'error';
        icon = "bi bi-exclamation-octagon-fill";
    }
    subtitle = subtitle ? subtitle  : '';
    delay = delay ? Number(delay) : 4000;

    $(document).Toasts('create', {
        class: 'bg-'+status,
        body: message,
        title: title,
        subtitle: subtitle,
        icon: icon,
        autohide: delay > 100,
        delay: delay,
    })
    // toastr[type](message, title);
}
function setColorZero() {
    $(`input[type=number]
    ,input[data-filter=number]
    ,input[data-filter=numbermin]
    ,input[data-filter=number2]
    ,input[data-filter=decimal]
    ,input[data-filter=decimal1]`).filter(':visible:not(:disabled):not([readonly])').each((index, el) => {
        const helper = $(el);
        if (Number(helper.val()) == 0) {
            helper.addClass('text-zero');
        }
    });
}

$(`input[type=number]
,input[data-filter=number]
,input[data-filter=numbermin]
,input[data-filter=number2]
,input[data-filter=decimal]
,input[data-filter=decimal1]`).filter(':visible:not(:disabled):not([readonly])').on('change', (el) => {
    const helper = $(el.target);
    if (Number(helper.val()) != 0) {
        helper.addClass('text-abs');
        helper.removeClass('text-zero');
    } else {
        helper.removeClass('text-abs');
        helper.addClass('text-zero');
    }
});

$(`input[type=number]
,input[data-filter=number]
,input[data-filter=numbermin]
,input[data-filter=number2]
,input[data-filter=decimal]
,input[data-filter=decimal1]`).filter(':visible:not(:disabled):not([readonly])').on('click', (el) => {
    const helper = $(el.target);
    if (Number(helper.val()) == 0) {
        helper.val('');
    }
});

$(`input[type=number]
,input[data-filter=number]
,input[data-filter=numbermin]
,input[data-filter=number2]
,input[data-filter=decimal]
,input[data-filter=decimal1]`).filter(':visible:not(:disabled):not([readonly])').on('blur', (el) => {
    const helper = $(el.target);
    let newval = isNaN(Number(helper.val())) ? 0 : Number(helper.val());
    helper.val(newval);
});


function inputFilterInit() {
    // Install input filters.
    // Integer (positive only)
    $("input[data-filter=number]").inputFilter(function(value) {
        return /^\d*$/.test(value);
    });
    // Integer (positive and <= 500):
    $("input[data-filter=numbermin]").inputFilter(function(value) {
        return /^\d*$/.test(value) && (value === "" || parseInt(value) <= 500);
    });
    // Integer (both positive and negative):
    $("input[data-filter=number2]").inputFilter(function(value) {
        return /^-?\d*$/.test(value);
    });
    // Floating point (use . or , as decimal separator):
    $("input[data-filter=decimal]").inputFilter(function(value) {
        return /^-?\d*[.,]?\d*$/.test(value);
    });
    // Currency (at most two decimal places):
    $("input[data-filter=decimal1]").inputFilter(function(value) {
        return /^-?\d*[.,]?\d{0,2}$/.test(value);
    });
    // A-Z only:
    $("input[data-filter=string]").inputFilter(function(value) {
        return /^[a-z]*$/i.test(value);
    });
    // Latin letters only (most European languages):
    $("input[data-filter=string2]").inputFilter(function(value) {
        return /^[a-z\u00c0-\u024f]*$/i.test(value);
    });
    // Hexadecimal:
    $("input[data-filter=string3]").inputFilter(function(value) {
        return /^[0-9a-f]*$/i.test(value);
    });

    $("input[data-filter=custom]").inputFilter(function(value) {
        let _regexp = $(event.target).attr('regexp');
        if (_regexp) {
            return RegExp(`^${_regexp}*$`, "i").test(value);
        }
        return true;
    });
}

function signaturePadResize() {
    $('canvas.signature-pad-canvase').each((index, el) => {
        el.width = $(el).closest('.signature-pad-box').eq(0).width() - 2;
        el.height = $(el).closest('.signature-pad-box').eq(0).height() - 2;
    });
}

function signaturePadInit() {
    $('canvas.signature-pad-canvase').each((index, el) => {
        let refid = $(el).attr('data-ref');
        signaturePad[refid] = new SignaturePad(el, {
            backgroundColor: 'rgba(0, 0, 0, 0)'            
        });
        signaturePad[refid].addEventListener("beginStroke", (e) => {
            $('button.signature-pad-clear[data-ref=' + refid + ']').off().on("click",(e) => {
                let clearrefid = $(e.target).attr('data-ref');
                signaturePad[clearrefid].clear();
                $("#" + clearrefid).val("");
            });
          });
        signaturePad[refid].addEventListener("endStroke", (e) => {
            var dataURL = signaturePad[refid].toDataURL();
            signaturePadDefaultText(el);
            $("#" + refid).val(dataURL);
          });
        signaturePadDefaultText(el);
        signaturePadResize();
    });


    function signaturePadDefaultText(el) {
        let _width = $(el).closest('.signature-pad-box').eq(0).width();
        let _height = $(el).closest('.signature-pad-box').eq(0).height();
        let _timestamp = $(el).attr("data-timestamp");
        let appname = $("meta[name=appName]").attr('content');
        el = el.getContext("2d");

        switch (_timestamp) {
            case "top":
                el.beginPath();
                el.moveTo(0, 20);
                el.lineTo(_width, 20);
                el.closePath();
                el.stroke();
                el.font = "10px bold Comic Sans MS";
                el.textAlign = "end";
                el.clearRect(0, 0, _width, 20);
                el.save();
                el.fillText(appname + " : " + moment().format('LLL'), (_width - 10), (15));
                break;
            case "bottom":
                el.beginPath();
                el.moveTo(0, (_height - 20));
                el.lineTo(_width, (_height - 20));
                el.closePath();
                el.stroke();
                el.font = "10px bold Comic Sans MS";
                el.textAlign = "end";
                el.clearRect(0, (_height - 20), _width, 20);
                el.save();
                el.fillText(appname + " : " + moment().format('LLL'), (_width - 10), (_height - 8));
                break;
            case "left":
                el.beginPath();
                el.moveTo(20, 0);
                el.lineTo(20, _height);
                el.closePath();
                el.stroke();
                el.font = "10px bold Comic Sans MS";
                el.textAlign = "end";
                el.clearRect(0, 0, 20, _width);
                el.save();
                el.rotate(0.5 * Math.PI);
                el.fillText(appname + " : " + moment().format('LLL'), (_height - 5), -8);
                el.restore();
                break;
            case "right":
                el.beginPath();
                el.moveTo((_width - 21), 0);
                el.lineTo((_width - 21), _height);
                el.closePath();
                el.stroke();
                el.font = "10px bold Comic Sans MS";
                el.textAlign = "end";
                el.clearRect((_width - 20), 0, 20, _width);
                el.save();
                el.rotate(-0.5 * Math.PI);
                el.fillText(appname + " : " + moment().format('LLL'), -5, (_width - 8));
                el.restore();
                break;

            default:
                el.beginPath();
                el.moveTo(0, (_height - 20));
                el.lineTo(_width, (_height - 20));
                el.closePath();
                el.stroke();
                el.font = "10px bold Comic Sans MS";
                el.textAlign = "end";
                el.clearRect(0, (_height - 20), _width, 20);
                el.save();
                el.fillText(appname + " : " + moment().format('LLL'), (_width - 10), (_height - 8));
                break;
        }
    }
}

function strMin($str) {
    return $str.replace(/^\s+|\r\n|\n|\r|(>)\s+(<)|\s+$/gm, '$1$2');
}

function popupPostTableData(_table, _url, dataObj) {
    let keyDataObj = typeof dataObj == 'object' ? Object.keys(dataObj) : [];
    let valDataObj = typeof dataObj == 'object' ? Object.values(dataObj) : [];
    let tableData = $(_table).html().replace(/<a>/g, '<span>')
                                    .replace(/<\/a>/g, '</span>')
                                    .replace(/^\s+|\r\n|\n|\r|(>)\s+(<)|\s+$/gm, '$1$2');
    _helper.popupPost(_url, 'exportExcel', null, ['dataExport', ...keyDataObj], [`${tableData}`, ...valDataObj]);
}

function tooltipInit() {
    $('.toolhover').each((i, el) => {
        if ($(el).find('li').length < 2) {
            $(el).remove();
        }
    });

    $('.togget-toolhover').off('click');
    $('.togget-toolhover').on('click', (e) => {
        let toolhover = $('.toolhover');
        if (toolhover.hasClass('toolhover-mini')) {
            toolhover.removeClass('toolhover-mini');
            toolhover.find('.icon-togget')
                .removeClass('fa-plus')
                .addClass('fa-minus');
        } else {
            toolhover.addClass('toolhover-mini');
            toolhover.find('.icon-togget')
                .removeClass('fa-minus')
                .addClass('fa-plus');
        }

    });
}


// custom jquery function plugin

(function($) {

    $.fn.componentAjaxView = function(options, _callback,_onerrors) {
        _callback = _callback || function() {};
        _onerrors = _onerrors || function() {};
        let settings = { id: '', url: '', query: '', method: 'get', page: '', updated: null };
        if (typeof options != 'function' && typeof options != 'string') {
            settings = $.extend({
                id: '',
                url: '',
                query: '',
                method: 'get',
                page: '',
                // event
                updated: null,
                errors: null
            }, options)
        }
        this.each(function() {
            const el = this;
            const _id = settings.id ? settings.id : $(el).attr('id');
            const targetName = this.localName + (_id ? '#' + _id : '') +
                ($(this).attr('class') ? '.' + $(this).attr('class').split(' ').join('.') : '');
            const _url = settings.url ? settings.url : $(el).attr('data-url');
            const _query = settings.query ? settings.query : $(el).attr('data-query');
            const _method = settings.method ? settings.method : $(el).attr('data-method');
            const _page = settings.page ? settings.page : $(el).attr('data-page');
            const _updated = $(el).attr('data-updated') ? $(el).attr('data-updated') : '';
            const _errors = $(el).attr('data-errors') ? $(el).attr('data-errors') : '';

            // loadView

            if ($.isFunction(settings.updated) || $.isFunction(settings.errors)) {
                loadView(_id, _url, _query, _page, _method, $.isFunction(settings.updated) ? settings.updated : _callback,$.isFunction(settings.errors) ? settings.errors : _onerrors);
                return;
            }

            // reload data
            if (typeof options == 'string' && options == 'reload') {
                _callback = _callback || function() {};
                loadView(_id, _url, _query, _page, _method, _callback, _onerrors);
                return;
            }

            loadView(_id, _url, _query, _page, _method, _callback, _onerrors);
            error_coount = 0;

            function loadView(id, url, query, page, method, callback, errors) {
                // if(!id) return alertBox('danger', 'คุณไม่ระบุบ ID Table ในการ Load Table');
                if (!url) return alertBox('danger', 'คุณไม่ระบุบ URL ในการ Load Table');
                page = page ? '&page=' + page : '';
                _helper.loading(targetName);
                query = decodeURIComponent(query);
                query = query ? query.split('?') : '?';
                query = Array.isArray(query) && query.length > 1 ? '?' + query[1] : '?';
                let encodeurl = (encodeURI(url + query + page)).replace(/\?$/,'');
                axios({
                    method: method,
                    url: encodeurl
                }).then((response) => {
                    let custom_responseURL = response.request.responseURL
                    if (custom_responseURL.indexOf(encodeurl) > -1) {
                        let data = response.data;
                        $(el).html(data).ready(() => {
                            _helper.updateInint(targetName);
                            $(el).attr('data-url', url);
                            $(el).attr('data-query', query);
                            $(el).attr('data-method', method);
                            $(el).attr('data-page', page);
                            const fullurl = url + query + page;
                            if (_updated) {
                                try {
                                    eval(_updated + "({target: el, url: fullurl, targetName})");
                                } catch (error) {}
                            }
                            if (typeof options == 'function') {
                                options({ target: el, url: fullurl, targetName });
                            }
                            callback({ target: el, url: fullurl, targetName });
                            _helper.stopLoading(targetName);
                            error_coount = 0;
                        })
                        error_coount = 0;
                    } else {
                        callOnError(callback, errors, null);
                    }
                }).catch((err) => {
                    callOnError(callback, errors, err.response);
                });

            }
            $(el).off('click', '.pagination a.page-link');
            $(el).on('click', '.pagination a.page-link', (e) => {
                e.preventDefault()
                if($(e.target).attr('aria-controls')){ return false; }
                let urlParams = new URLSearchParams($(e.target).attr('href'));
                let page = urlParams.get('page') || '';
                loadView(_id, _url, _query, page, _method, _callback);
            })

            function callOnError(fcallback, ferrors, message) {
                error_coount = error_coount + 1;
                if (error_coount <= 5) {
                    setTimeout(() => {
                        console.log('error.count', error_coount);
                        loadView(_id, _url, _query, _page, _method, fcallback,ferrors);
                    }, 500);
                } else {
                    alertBox('danger', `เกิดข้อผิดพลาดในการทำงาน ของ ${targetName}
                                <button class="btn btn-xs" onclick="$('${targetName}').componentAjaxView('reload');">
                                <i class="fa fa-refresh" aria-hidden="true"></i>
                                </button>
                            `);
                    if (_errors) {
                        try {
                            eval(_errors + "(message)");
                        } catch (error) {}
                    }
                    ferrors(message);
                    _helper.stopLoading(targetName);
                }
            }

        })
        return this;
    };

    $.fn.formAjax = function(options, _callback, _onerrors) {
        let settings = { id: '', url: '', method: '', updated: null };
        if (typeof options != 'function' && typeof options != 'string') {
            settings = $.extend({
                id: '',
                url: '',
                method: '',
                updated: null
            }, options)
        }
        this.each(function() {
            const el = this;
            const _id = settings.id ? settings.id : $(el).attr('id');
            const targetName = this.localName + (_id ? '#' + _id : '') +
                ($(this).attr('class') ? '.' + $(this).attr('class').split(' ').join('.') : '');
            let _url = settings.url ? settings.url : $(el).attr('action');
            const _method = settings.method ? settings.method : ($(el).attr('method') ? $(el).attr('method') : 'post');
            const _updated = $(el).attr('data-updated') ? $(el).attr('data-updated') : '';
            const _errors = $(el).attr('data-errors') ? $(el).attr('data-errors') : '';

            if ($(el).prop("tagName") !== 'FORM') return alertBox('danger', '.formAjax: ใช้ได้กับ form เท่านั้น');
            if (!_url) return alertBox('danger', '.formAjax: คุณไม่ระบุบ URL ในการ action form');
            // if(!$(el).has('.form-body').length) return alertBox('danger', '.formAjax: ไม่พบ .form-body ภายใต้ .formAjax');
            $(el).off('submit');
            $(el).on('submit', function(e) {
                _callback = _callback || function() {};
                _onerrors = _onerrors || function() {};
                e.preventDefault();

                //set data to FormData
                const helperform = $(e.target)[0];
                try { if(!helperform.valid()) return false; } catch (error) { }
                let formData = null;
                if (_method == 'get' || _method == 'GET') {
                    _url = _url + '?' + $(helperform).serialize();
                } else {
                    const helper_input_file = $(e.target).find('input[type=file]'); // find input type file
                    formData = new FormData(helperform);
                    //set data file
                    helper_input_file.each((index, item) => {
                        formData.append($(item).attr('name'), $(item)[0].files[0]); // add file to FormData
                    });
                }
                
                _helper.loading(targetName + ($(e.target).has('.form-loading').length ? ' .form-loading' : ''));
                
                //send ajax to server
                axios({
                        method: _method,
                        url: encodeURI(_url),
                        data: formData
                    }).then((response) => {
                        let homepage = $('meta[name=homepage').attr('content');
                        let custom_responseURL = response.request.responseURL;
                        if (custom_responseURL != encodeURI(homepage)) {
                            let data = response.data;
                            if ($(e.target).has('.form-body').length) {
                                $(e.target).find('.form-body').html(data).ready(() => {
                                    const status = $(e.target).has('.error-message').length ? 'error' : 'success';
                                    $(e.target).attr('action', _url);
                                    $(e.target).attr('method', _method);
                                    $(e.target).attr('data-status', status);
                                    if (_updated) {
                                        try {
                                            eval(_updated + "({target: e.target, status, targetName, data})");
                                        } catch (error) {}
                                    }
                                    if (typeof options == 'function') {
                                        options({ target: e.target, status, targetName, data });
                                    }
                                    _callback({ target: e.target, status, targetName, data });
                                    _helper.stopLoading(targetName + ($(e.target).has('.form-loading').length ? ' .form-loading' : ''));
                                })
                            } else {
                                const status = $(e.target).has('.error-message').length ? 'error' : 'success';
                                $(e.target).attr('action', _url);
                                $(e.target).attr('method', _method);
                                $(e.target).attr('data-status', status);
                                if (_updated) {
                                    try {
                                        eval(_updated + "({target: e.target, status, targetName, data})");
                                    } catch (error) {}
                                }
                                if (typeof options == 'function') {
                                    options({ target: e.target, status, targetName, data });
                                }
                                _callback({ target: e.target, status, targetName, data });
                                _helper.stopLoading(targetName + ($(e.target).has('.form-loading').length ? ' .form-loading' : ''));
                            }
                        } else {
                            alertBox('danger', `code f001 เกิดข้อผิดพลาดในการทำงาน ของ ${targetName}
                                <button class="btn btn-xs" onclick="location.reload()">
                                <i class="fa fa-refresh" aria-hidden="true"></i>
                                </button>
                            `);
                            _helper.stopLoading(targetName + ($(e.target).has('.form-loading').length ? ' .form-loading' : ''));
                        }
                    })
                    .catch((err) => {
                        alertBox('danger', `code f002 เกิดข้อผิดพลาดในการทำงาน ของ ${targetName}
                                <button class="btn btn-xs" onclick="location.reload()">
                                <i class="fa fa-refresh" aria-hidden="true"></i>
                                </button>
                            `);
                        if (_errors) {
                            try {
                                eval(_errors + "(err.response)");
                            } catch (error) {}
                        }
                        _onerrors(err.response);
                        _helper.stopLoading(targetName + ($(e.target).has('.form-loading').length ? ' .form-loading' : ''));
                    });
            });

        })
        return this;
    };

    $.fn.checkPermission = function() {
        this.each((index, el) => {
            let elPermissions = $(el).attr('can').trim();
            elPermissions = elPermissions.replace(/\[|\]/g, '');
            let mergeRolePermissions = [..._myRoles, ..._myPermissions];
            if (elPermissions && elPermissions != '[]') {
                elPermissions = elPermissions.split(',');
                let IsAllow = false;
                elPermissions.forEach((item, index) => {
                    if (mergeRolePermissions.indexOf(item.trim()) >= 0) {
                        return IsAllow = true;
                    }
                });
                if (!IsAllow) {
                    $(el).hide();
                }
            }
        });
    }

    // Restricts input for each element in the set of matched elements to the given inputFilter.

    $.fn.inputFilter = function(inputFilter) {
        return this.on("input keydown keyup mousedown mouseup select contextmenu drop", function() {
            if (inputFilter(this.value)) {
                this.oldValue = this.value;
                this.oldSelectionStart = this.selectionStart;
                this.oldSelectionEnd = this.selectionEnd;
            } else if (this.hasOwnProperty("oldValue")) {
                this.value = this.oldValue;
                this.setSelectionRange(this.oldSelectionStart, this.oldSelectionEnd);
            }
        });
    };

    $.fn.hScroll = function(amount) {
        amount = amount || 120;
        $(this).bind("DOMMouseScroll mousewheel", function(event) {
            var oEvent = event.originalEvent,
                direction = oEvent.detail ? oEvent.detail * -amount : oEvent.wheelDelta,
                position = $(this).scrollLeft();
            position += direction > 0 ? -amount : amount;
            $(this).scrollLeft(position);
            event.preventDefault();
        })
    };

}(jQuery));

// call custom function

$('.componentAjaxView').componentAjaxView();
$('.formAjax').formAjax();

// add validation filesize
$.validator.addMethod("filesize", function (value, element, param) {
    if (value === "") {
        return true;
    }
    var maxBytes = parseInt(param);
    if (element.files !== undefined && element.files[0] !== undefined && element.files[0].size !== undefined) {
        console.log('Dumping the file object', element.files[0]);
        var filesize = parseInt(element.files[0].size);
        return filesize <= maxBytes;
    }
    return true;
});

$.validator.unobtrusive.adapters.add('filesize', ['maxfilesize'], function (options) {
    // set the parameter
    options.rules['filesize'] = options.params.maxfilesize;
    if (options.message) {
        // If there is a message, set it for the rule
        options.messages['filesize'] = options.message;
    }
});