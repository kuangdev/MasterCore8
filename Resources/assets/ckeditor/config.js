/**
 * @license Copyright (c) 2003-2019, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see https://ckeditor.com/legal/ckeditor-oss-license
 */
CKEDITOR.timestamp=document.querySelector('meta[name=cacheVersion]').getAttribute("content");
CKEDITOR.editorConfig = function(config) {
    // Define changes to default configuration here. For example:
    config.language = 'th';
    // config.uiColor = '#AADC6E';
    config.toolbar = [
        { name: 'tools', items: ['Maximize', 'ShowBlocks'] },
        { name: 'document', groups: ['mode', 'document', 'doctools'], items: ['Source', '-', 'Save', 'NewPage', 'Preview', 'Print', '-', 'Templates'] },
        { name: 'clipboard', groups: ['clipboard', 'undo'], items: ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Undo', 'Redo'] },
        { name: 'editing', groups: ['find', 'selection', 'spellchecker'], items: ['Find'] },
        // { name: 'forms', items: [ 'Form', 'Checkbox', 'Radio', 'TextField', 'Textarea', 'Select', 'Button', 'ImageButton', 'HiddenField' ] },
        { name: 'insert', items: ['Image', 'Link', 'Html5video', 'Youtube', 'HorizontalRule', 'SpecialChar', 'PageBreak', 'Iframe'] },
        // { name: 'links', items: [ 'Link', 'Unlink', 'Anchor' ] },
        '/',
        { name: 'paragraph', groups: ['list', 'indent', 'blocks', 'align', 'bidi'], items: ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', '-', 'Blockquote', 'CreateDiv', '-', 'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock', '-'] },
        { name: 'styles', items: ['Styles', 'Format', 'Font', 'FontSize'] },
        { name: 'colors', items: ['TextColor', 'BGColor'] },
        { name: 'basicstyles', groups: ['basicstyles', 'cleanup'], items: ['Bold', 'Italic', 'Underline', 'Strike', 'Subscript', 'Superscript', '-', 'CopyFormatting', 'RemoveFormat'] },
        '/',
        { name: 'tables', items: ['Table', 'tableinsert', 'tabledelete', 'tableproperties', 'tablerowinsertbefore', 'tablerowinsertafter', 'tablerowdelete', 'tablecolumninsertbefore', 'tablecolumninsertafter', 'tablecolumndelete', 'tablecellsmerge'] }
    ];

    config.toolbar_full = [
        { name: 'tools', items: ['Maximize', 'ShowBlocks'] },
        { name: 'document', groups: ['mode', 'document', 'doctools'], items: ['Source', '-', 'Save', 'NewPage', 'Preview', 'Print', '-', 'Templates'] },
        { name: 'clipboard', groups: ['clipboard', 'undo'], items: ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Undo', 'Redo'] },
        { name: 'editing', groups: ['find', 'selection', 'spellchecker'], items: ['Find'] },
        // { name: 'forms', items: [ 'Form', 'Checkbox', 'Radio', 'TextField', 'Textarea', 'Select', 'Button', 'ImageButton', 'HiddenField' ] },
        { name: 'insert', items: ['Image', 'Link', 'Html5video', 'Youtube', 'HorizontalRule', 'SpecialChar', 'PageBreak', 'Iframe'] },
        // { name: 'links', items: [ 'Link', 'Unlink', 'Anchor' ] },
        '/',
        { name: 'paragraph', groups: ['list', 'indent', 'blocks', 'align', 'bidi'], items: ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', '-', 'Blockquote', 'CreateDiv', '-', 'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock', '-'] },
        { name: 'styles', items: ['Styles', 'Format', 'Font', 'FontSize'] },
        { name: 'colors', items: ['TextColor', 'BGColor'] },
        { name: 'basicstyles', groups: ['basicstyles', 'cleanup'], items: ['Bold', 'Italic', 'Underline', 'Strike', 'Subscript', 'Superscript', '-', 'CopyFormatting', 'RemoveFormat'] },
        '/',
        { name: 'tables', items: ['Table', 'tableinsert', 'tabledelete', 'tableproperties', 'tablerowinsertbefore', 'tablerowinsertafter', 'tablerowdelete', 'tablecolumninsertbefore', 'tablecolumninsertafter', 'tablecolumndelete', 'tablecellsmerge'] }
    ];

    config.toolbar_nomal = [
		{ name: 'basicstyles', items: [ 'Bold', 'Italic', 'Underline','Strike', '-','CopyFormatting', 'RemoveFormat', '-'] },
		{ name: 'paragraph', items: [ 'NumberedList', 'BulletedList', 'Outdent', 'Indent', 'Blockquote', 'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock', '-'] },
		'-',{ name: 'insert', items: [ 'Table','-','Image', 'Link', '-'] },
        '/',
        { name: 'styles', items: [ 'Styles', 'Format', 'Font', 'FontSize', '-'] },
		{ name: 'colors', items: [ 'TextColor', 'BGColor', '-' ] }
	];

    config.toolbar_mini = [
		{ name: 'basicstyles', items: [ 'Bold', 'Italic', 'Underline' ] },
		{ name: 'paragraph', items: [ 'NumberedList', 'BulletedList' ] },
		{ name: 'insert', items: [ 'Table'] },
		{ name: 'colors', items: [ 'TextColor', 'BGColor' ] }
	];

    config.format_tags = 'p;h1;h2;h3;pre';
    config.extraPlugins = 'tabletoolstoolbar,tableresize,floatpanel,panelbutton,quicktable,btquicktable';
    config.removePlugins = 'easyimage,simage,emoji,cloudservices';

    var CKUploadUrl = document.querySelector('meta[name=CKUploadUrl]').getAttribute("content").trim("/");
    // var moduleName = document.querySelector('meta[name=moduleName]').getAttribute("content");
    var _token = document.querySelector('input[name=__RequestVerificationToken]').value;
    // let moduleNamePath = (moduleName && moduleName != "home") ? "/"+moduleName : "";
    // config.filebrowserUploadUrl = burl + '/ckupload'+ moduleNamePath +'?_token=' + _token;
    config.filebrowserUploadUrl = CKUploadUrl + '?_token=' + _token;
    config.filebrowserUploadMethod = 'form';
    config.image_removeLinkByEmptyURL = true;
    config.image_previewText = CKEDITOR.tools.repeat('ตัวอย่างรูปภาพ ', 100);
    image2_alignClasses: ['image-align-left', 'image-align-center', 'image-align-right'],
        // Simplify the dialog windows.
        config.removeDialogTabs = 'image:advanced;link:advanced';
};
