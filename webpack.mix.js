const mix = require('laravel-mix');


mix.styles([
    // 'node_modules/admin-lte/plugins/fontawesome-free/css/all.css',
    // 'node_modules/font-awesome/css/font-awesome.css',
    'node_modules/bootstrap-icons/font/bootstrap-icons.css',
    'node_modules/flatpickr/dist/flatpickr.css',
    'node_modules/flatpickr/dist/plugins/monthSelect/style.css',
    'node_modules/admin-lte/plugins/select2/css/select2.css',
    'node_modules/admin-lte/plugins/select2-bootstrap4-theme/select2-bootstrap4.css',
    'node_modules/datatables.net-bs4/css/dataTables.bootstrap4.css',
    'node_modules/admin-lte/plugins/bs-stepper/css/bs-stepper.css',
    // 'node_modules/admin-lte/plugins/sweetalert2-theme-bootstrap-4/bootstrap-4.css',
    'node_modules/sweetalert2/dist/sweetalert2.css',
    'node_modules/admin-lte/dist/css/adminlte.css',
    'node_modules/glightbox/dist/css/glightbox.css',
    'Resources/css/google-fonts.css',
    'Resources/css/custom.css',
], 'wwwroot/css/master-app.css');


mix.js('Resources/js/app.js', 'wwwroot/js/app.js');

mix.combine(['wwwroot/js/app.js',
// 'node_modules/jquery/dist/jquery.js',
'node_modules/admin-lte/plugins/jquery/jquery.js',
'node_modules/admin-lte/plugins/bootstrap/js/bootstrap.bundle.js',
'node_modules/admin-lte/plugins/moment/moment.min.js',
'node_modules/admin-lte/plugins/moment/locale/th.js',
'node_modules/flatpickr/dist/flatpickr.js',
'node_modules/flatpickr/dist/plugins/monthSelect/index.js',
'node_modules/flatpickr/dist/l10n/th.js',
'node_modules/admin-lte/plugins/select2/js/select2.full.js',
'node_modules/admin-lte/plugins/bs-stepper/js/bs-stepper.js',
'node_modules/admin-lte/plugins/jquery-validation/jquery.validate.js',
'node_modules/admin-lte/plugins/jquery-validation/additional-methods.js',
// 'node_modules/admin-lte/plugins/sweetalert2/sweetalert2.all.js',
'node_modules/sweetalert2/dist/sweetalert2.all.js',
'node_modules/admin-lte/dist/js/adminlte.js',
'node_modules/jquery-equal-height/js/jquery-equal-height.js',
// 'node_modules/jquery-validation/dist/jquery.validate.js',
'node_modules/jquery-validation-unobtrusive/dist/jquery.validate.unobtrusive.js',
'node_modules/jquery-ajax-unobtrusive/dist/jquery.unobtrusive-ajax.js',
'node_modules/datatables.net/js/jquery.dataTables.js',
'node_modules/datatables.net-bs4/js/dataTables.bootstrap4.js',
'node_modules/floatthead/dist/jquery.floatThead.js',
'node_modules/glightbox/dist/js/glightbox.js',
'node_modules/inputmask/dist/jquery.inputmask.js',
'node_modules/signature_pad/dist/signature_pad.umd.js',
'Resources/assets/bootstrap-autocomplete-master/dist/js/bootstrap-autocomplete.js',
'Resources/assets/ckeditor/basepath.js',
'Resources/assets/ckeditor/ckeditor.js',
'node_modules/underscore/underscore-min.js',
'node_modules/file-saver/dist/FileSaver.js',
'node_modules/xlsx/dist/xlsx.full.min.js',
'Resources/js/helpers.js',
], 'wwwroot/js/master-app.js');


mix.copyDirectory('Resources/assets/ckeditor', 'wwwroot/js/ckeditor');
mix.copyDirectory('Resources/assets/fonts','wwwroot/fonts');
mix.copyDirectory('node_modules/admin-lte/dist/img','wwwroot/img');
mix.copyDirectory('node_modules/bootstrap-icons/font/fonts', 'wwwroot/css/fonts');
mix.copyDirectory('node_modules/admin-lte/plugins/fontawesome-free/webfonts', 'wwwroot/webfonts');
mix.copyDirectory('node_modules/font-awesome/fonts','wwwroot/fonts');


// more plugins
mix.copy('node_modules/sortablejs/Sortable.min.js','wwwroot/js');
mix.copy('Resources/css/pdf-fonts.css','wwwroot/css');
// add npm i chart.js 
mix.copyDirectory('node_modules/chart.js/dist/chart.min.js','wwwroot/js');
