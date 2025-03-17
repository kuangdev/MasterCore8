using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;

/* Example Code
<formitem 
    col = "col-md-6" type = "text" label = "@Html.DisplayNameFor(model=>model.user_id)" show-label = "true" 
    asp-for = "user_id" id = "form-@(formType)-user_id" Name = "user_id" 
    value = "@User.FindFirst("id").Value" class = "" icon = ""  
    inline = "FormInline" form-group = "true" required = "true" readonly="true" 
    option="" />
*/

namespace MasterCore8.Helpers.TagHelpers
{

    [HtmlTargetElement("formitem")]
    public class FormItemTagHelper_20221026 : TagHelper
    {
        public string Type { get; set; } 
        public string Id { get; set; }
        public string Name { get; set; } 
        public bool FormGroup { get; set; } = true;
        public string Value { get; set; } 
        public string Label { get; set; } 
        public bool ShowLabel { get; set; } = true;
        public string Class { get; set; } 
        public string Icon { get; set; } 
        public bool Inline { get; set; } = false;
        public bool RadioInline { get; set; } = false;
        public string Col { get; set; } 
        public bool Select2 { get; set; } = true;
        public bool Required { get; set; } = false;
        public bool Checked { get; set; } = false;

        // select & radio
        public Dictionary<string, string> Option { get; set; }
        public List<Dictionary<string, string>> Optionattr { get; set; }

        // Textarea & editor & signature
        public int Rows { get; set; } = 3;
        public string Width { get; set; }
        public string Height { get; set; }
        // [ top , bottom , left , right ]
        public string Timestamp { get; set; } = "bottom";

        private readonly IHtmlGenerator _generator;
        [ViewContext]
        public ViewContext ViewContext { get; set; }
        [HtmlAttributeName("asp-for")]
        public ModelExpression For { get; set; }

        public FormItemTagHelper_20221026(IHtmlGenerator generator)
        {
            _generator = generator;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if(!Checked) Checked = false;
            if(string.IsNullOrEmpty(Id)) Id = For?.Name ?? Guid.NewGuid().ToString("N").Substring(0, 5);
            if(string.IsNullOrEmpty(Name)) Name = For?.Name ?? "";
            if(string.IsNullOrEmpty(Value)){
                Value = "";
                if(For?.Model != null) Value=For.Model.ToString();
            };
            string addAttribute = extraAttribute(context);
            string formgroup = ""; 
            var input = new TagBuilder("input");
            output.TagName = "";        
            if(Option == null) Option = new Dictionary<string, string>();
            if(Optionattr == null) Optionattr = new List<Dictionary<string, string>>();
            //set default position timestamp
            if((new string[] {"top" , "bottom" , "left" , "right"}).Contains(Timestamp)){
                Timestamp = "bottom";
            }
            output.SuppressOutput();
            output.Content.Clear();
            
            // ========= set type input ==============
            // date,datetime, month, time,daterange file, textarea, editor, checkbox, switch, radio, select, signature, autocomplete
            switch (Type.ToLower())
            {
                case "hidden":
                    input = CreateInput(For,context);
                    input.Attributes["type"]="hidden";
                    output.Content.AppendHtml(input);
                    break;
                case "password":
                    input = CreateInput(For,context);
                    input.Attributes["type"]="password";
                    input.Attributes["value"]="";
                    formgroup = CreateFormGroup(Type, ConvertString(input));
                    output.Content.AppendHtml(formgroup);
                    break;
                case "date":
                    input = CreateInput(For,context);
                    input.Attributes["class"] = "form-control datepicker "+Class;
                    input.Attributes["type"]="text";
                    if(string.IsNullOrEmpty(Icon)){
                        Icon = "bi bi-calendar-week";
                    }
                    formgroup = CreateFormGroup(Type, ConvertString(input));
                    output.Content.AppendHtml(formgroup);
                    break;
                case "datetime":
                    input = CreateInput(For,context);
                    input.Attributes["class"] = "form-control datetimepicker "+Class;
                    input.Attributes["type"]="text";
                    if(string.IsNullOrEmpty(Icon)){
                        Icon = "bi bi-calendar-week";
                    }
                    formgroup = CreateFormGroup(Type, ConvertString(input));
                    output.Content.AppendHtml(formgroup);
                    break;
                case "month":
                    input = CreateInput(For,context);
                    input.Attributes["class"] = "form-control monthpicker "+Class;
                    input.Attributes["type"]="text";
                    if(string.IsNullOrEmpty(Icon)){
                        Icon = "bi bi-calendar-week";
                    }
                    formgroup = CreateFormGroup(Type, ConvertString(input));
                    output.Content.AppendHtml(formgroup);
                    break;
                case "time":
                    input = CreateInput(For,context);
                    input.Attributes["class"] = "form-control timepicker "+Class;
                    input.Attributes["type"]="text";
                    if(string.IsNullOrEmpty(Icon)){
                        Icon = "bi bi-calendar-week";
                    }
                    formgroup = CreateFormGroup(Type, ConvertString(input));
                    output.Content.AppendHtml(formgroup);
                    break;
                case "daterange":
                    input = CreateInput(For,context);
                    input.Attributes["class"] = "form-control daterangepicker "+Class;
                    input.Attributes["type"]="text";
                    if(string.IsNullOrEmpty(Icon)){
                        Icon = "bi bi-calendar-week";
                    }
                    formgroup = CreateFormGroup(Type, ConvertString(input));
                    output.Content.AppendHtml(formgroup);
                    break;
                case "file":
                    input = CreateInput(For,context);
                    if(string.IsNullOrEmpty(Icon)){
                        Icon = "bi bi-paperclip";
                    }
                    formgroup = CreateFormGroup(Type, ConvertString(input));
                    output.Content.AppendHtml(formgroup);
                    break;
                case "textarea":
                    input = CreateTextarea(For,context);
                    input.Attributes["rows"] = Rows.ToString();
                    input.Attributes["style"]="resize:none;";
                    formgroup = CreateFormGroup(Type, ConvertString(input));
                    output.Content.AppendHtml(formgroup);
                    break;
                case "editor":
                    input = CreateTextarea(For,context);
                    input.Attributes["rows"] = Rows.ToString();
                    input.Attributes["style"]="resize:none;display:none";
                    formgroup = CreateFormGroup(Type, ConvertString(input));
                    output.Content.AppendHtml(formgroup);
                    string genJs = @$"
                    <script>
                        setTimeout(() => {{
                            CKEDITOR.replace( '{Id}',{{
                                toolbar: $('#{Id}').attr('toolbar') ? $('#{Id}').attr('toolbar') : 'Full'
                            }});
                        }}, 600);
                    </script>
                    ";
                    output.Content.AppendHtml(genJs);
                    break;
                case "checkbox":
                    // old version 
                    // txtCheckbox = @$"
                    //     <div class='{(FormGroup == true ? "form-group" : "")}'>
                    //         <label class='d-flex align-items-center control-label {( !string.IsNullOrEmpty(Label) ? "mr-2" : "" )} {Col}'>
                    //         <input type='{Type}' class='mr-2 { Class }' id='{ Id }' name='{ Name }' value='{ Value }' { (Checked ? "checked" : "" )} {extraAttribute(context)}>
                    //         { Label } {(Required ? "<span class='ml-1 text-danger required-text'>*</span>" : "" )}
                    //         </label>
                    //     </div>";          

                    output.Content.AppendHtml(@$"
                        <div class='{(FormGroup == true ? "form-group" : "")}'>
                            <div class='custom-control custom-checkbox'>
                                <input type='checkbox' class='custom-control-input { Class }' id='{Id}' name='{ Name }' value='{ Value }' { (Checked ? "checked" : "" )} {extraAttribute(context)}>
                                <label class='custom-control-label {( !string.IsNullOrEmpty(Label) ? "" : "" )} {Col}' for='{Id}'>{ Label } {(Required ? "<span class='ml-1 text-danger required-text'>*</span>" : "" )}</label>
                            </div>                            
                        </div>");
                    break;
                case "switch":
                    // old version 
                    // txtCheckbox = @$"
                    //     <div class='{(FormGroup == true ? "form-group" : "")}'>
                    //         <label class='d-flex align-items-center control-label {( !string.IsNullOrEmpty(Label) ? "mr-2" : "" )} {Col}'>
                    //         <input type='{Type}' class='mr-2 { Class }' id='{ Id }' name='{ Name }' value='{ Value }' { (Checked ? "checked" : "" )} {extraAttribute(context)}>
                    //         { Label } {(Required ? "<span class='ml-1 text-danger required-text'>*</span>" : "" )}
                    //         </label>
                    //     </div>";          

                    output.Content.AppendHtml(@$"
                        <div class='{(FormGroup == true ? "form-group" : "")}'>
                            <div class='custom-control custom-switch'>
                                <input type='checkbox' class='custom-control-input { Class }' id='{Id}' name='{ Name }' value='{ Value }' { (Checked ? "checked" : "" )} {extraAttribute(context)}>
                                <label class='custom-control-label  {( !string.IsNullOrEmpty(Label) ? "" : "" )} {Col}' for='{Id}'>{ Label } {(Required ? "<span class='ml-1 text-danger required-text'>*</span>" : "" )}</label>
                            </div>                            
                        </div>");
                    break;
                case "radio":
                    using (var html = new System.IO.StringWriter())
                    {
                        html.Write(@$"<div class='{ (FormGroup ? "form-group" : "") }'>");
                        if(ShowLabel){
                            html.Write(@$"<label class='{ Col } { (Inline ? "" : "d-block") }'> { Label } { (Required ? "<span class='ml-1 text-danger required-text'>*</span>" : "" ) } :</label>");
                        }
                        int i=0;
                        var empltyValue = (string.IsNullOrEmpty(Value) && !(Option.ContainsKey("")));
                        
                        var radioChecked = "";
                        foreach (var item in Option)
                        {
                            var OptionattrDr = (Optionattr.Count > 0) ? Optionattr?.ElementAt(i) : null;
                            string OptionattrTxt = "";
                            if(OptionattrDr != null){
                                OptionattrTxt = string.Join(" ", OptionattrDr.Select(x => @$"{x.Key} = ""{x.Value}""").ToArray());
                            }
                            if(empltyValue && i==0){
                                radioChecked = "checked";
                            }else{
                                radioChecked = (item.Key == Value ? "checked" : "");
                            }
                            html.Write(@$"
                            <div class='custom-control custom-radio { (RadioInline ? "custom-control-inline" : "") }'>
                                <input type='radio' id='{ Id+item.Key }' name='{ Name }' class='custom-control-input { Class }' { radioChecked } value='{ item.Key }' { OptionattrTxt }>
                                <label class='custom-control-label { ((Inline && RadioInline) ? "" : "ml-4") }' for='{ Id+item.Key }'>{ item.Value }</label>
                            </div>
                            ");

                            // oldversion
                            // html.Write(@$"
                            // <label class='control-label label-radio mr-2 { Col } { (FormGroup ? "d-block" : "") }'>
                            //     <input type='radio' class='{ Class }' id='{ Id+item.Key }' name='{ Name }' { (item.Key == Value ? "checked" : "") } value='{ item.Key }' { OptionattrTxt } { (Required ? "required" : "") }>
                            //     { item.Value }
                            // </label>
                            // ");
                            i++;
                        }
                        html.Write("</div>");

                        output.Content.AppendHtml(html.ToString());
                        break;
                    }
                case "select":

                    using (var html = new System.IO.StringWriter())
                    {
                        html.Write(@$"<select id='{ Id }' name='{ Name }' class='form-control {(Inline ? "form-control-inline" : "")} {(Option.Count > 10 && Select2 ? "select2" : "")} {Class}' data-name='{ Name }' data-value='{ Value }' { extraAttribute(context) } { (Required ? "required" : "") }>");
                        html.Write(@$"<option value='' {(Value == "" ? "selected" : "")} >-- { Label } --</option>");
                        int i = 0;
                        foreach (var item in Option)
                        {
                            var OptionattrDr = (Optionattr.Count > 0) ? Optionattr?.ElementAt(i) : null;
                            string OptionattrTxt = "";
                            if(OptionattrDr != null){
                                OptionattrTxt = string.Join(" ", OptionattrDr.Select(x => @$"{x.Key} = ""{x.Value}""").ToArray());
                            }
                            html.Write(@$"<option value='{ item.Key }' {OptionattrTxt} {(Value == item.Key ? "selected" : "")}>{ item.Value }</option>");
                            i++;
                        }
                        html.Write(@$"</select>");
                        formgroup = CreateFormGroup(Type, html.ToString());
                        output.Content.AppendHtml(formgroup);                    
                        break;
                    }
                    


                case "signature": 
                
                    var merge = new StringBuilder();

                    input = CreateTextarea(For,context);
                    string style = "resize:none;";
                    style += (Required) ? "display: block;height: 0px;opacity: 0;padding: 0px;" : "display: none;";
                    input.Attributes["style"]=style;

                    string signatureBody = @$"
                        <div class=""signature-pad-main"" style=""{ (!string.IsNullOrEmpty(Width) ? "width:"+Width : "") }"">
                            <div class=""signature-pad-box"" style=""{ (!string.IsNullOrEmpty(Width) ? "width:"+Width : "") } { (!string.IsNullOrEmpty(Height) ? "height:"+Height : "") }"">
                                <canvas class=""signature-pad-canvase""
                                    data-ref=""{ Id }""
                                    data-timestamp=""{ Timestamp }"">
                                </canvas>
                            </div>
                            <button type=""button"" class=""signature-pad-clear btn btn-sm btn-default btn-block"" data-ref=""{Id}""> <i class=""bi bi-eraser"" aria-hidden=""true""></i> ลบเขียนใหม่</button>
                        </div>
                    ";
                    merge.AppendLine(ConvertString(input));
                    merge.AppendLine(signatureBody);
                    formgroup = CreateFormGroup(Type, merge.ToString());
                    // output
                    output.Content.AppendHtml(formgroup);
                    break;

                case "autocomplete":
                    input = CreateInput(For,context);
                    input.Attributes["class"] = "form-control autocomplete "+Class;
                    input.Attributes["value"]=Value;
                    input.Attributes["autocomplete"] = "off";
                    formgroup = CreateFormGroup(Type, ConvertString(input));
                    output.Content.AppendHtml(formgroup);
                    break;

                default:
                    // code block
                    input = CreateInput(For,context);
                    formgroup = CreateFormGroup(Type, ConvertString(input));
                    output.Content.AppendHtml(formgroup);                    
                    break;
            } 
        }

        private TagBuilder CreateInput(ModelExpression _For, TagHelperContext _context){
            TagBuilder input = new TagBuilder("input");
            if(_For!=null){
                input = _generator.GenerateTextBox(ViewContext,
                        _For.ModelExplorer,
                        _For.Name,
                        _For.Model,
                        null,
                        new { @class = "form-control",@id= Id });
                input.Attributes["type"] = Type;
                input.Attributes["name"] = Name;
                input.Attributes["value"] = Value;
            }else{
                input.Attributes["id"] = Id;
                input.Attributes["class"] = "form-control";
                input.Attributes["type"] = Type;
                input.Attributes["name"] = Name;
                input.Attributes["value"] = Value;
            }
            input.Attributes["placeholder"] = Label;
            if(Inline) input.AddCssClass("form-control-inline");
            if(Required) input.Attributes["required"] = "true";
            input.AddCssClass(Class);
            // ------------------- Begin Extra Attributes --------------------
            var attributeObjects = _context.AllAttributes.ToList();
            var props = this.GetType().GetProperties().Select(p => p.Name.Replace("_","").ToLower());
            attributeObjects.RemoveAll(a => props.Contains(a.Name.Replace("-","").ToLower()));
            foreach (var attr in attributeObjects)
            {
                if(attr.Name != "asp-for"){
                    input.Attributes[attr.Name] = attr.Value.ToString();
                }
            }            
            // ------------------- End Extra Attributes --------------------
            return input;
        }

        private TagBuilder CreateTextarea(ModelExpression _For, TagHelperContext _context){
            TagBuilder input = new TagBuilder("textarea");
            if(_For!=null){
                input = _generator.GenerateTextArea(ViewContext,
                        _For.ModelExplorer,
                        _For.Name,
                        Rows,
                        0,
                        new { @class = "form-control",@id= Id });
                input.Attributes["name"] = Name;
                input.InnerHtml.SetContent(Value);
            }else{
                input.Attributes["id"] = Id;
                input.Attributes["class"] = "form-control";
                input.Attributes["name"] = Name;
                input.InnerHtml.SetContent(Value);
            }
            input.Attributes["placeholder"] = Label;
            if(Inline) input.AddCssClass("form-control-inline");            
            if(Required) input.Attributes["required"] = "true";
            input.AddCssClass(Class);
            // ------------------- Begin Extra Attributes --------------------
            var attributeObjects = _context.AllAttributes.ToList();
            var props = this.GetType().GetProperties().Select(p => p.Name.Replace("_","").ToLower());
            attributeObjects.RemoveAll(a => props.Contains(a.Name.Replace("-","").ToLower()));
            foreach (var attr in attributeObjects)
            {
                if(attr.Name != "asp-for"){
                    input.Attributes[attr.Name] = attr.Value.ToString();
                }
            }            
            // ------------------- End Extra Attributes --------------------
            return input;
        }

        
        private string CreateFormGroup(string _Type, string input){
            using (var html = new System.IO.StringWriter())
            {

                if((new string[] {"date","datetime","month","time","daterange"}).Contains(_Type)){
                    html.Write(@$"<div class='{(Type == "signature" ? "signature-pad-set" : "")} {Col}'>");
                    html.Write(@$"<div class='{(FormGroup == true ? "form-group" : "")}'>");
                    if(ShowLabel && Inline==false) html.Write(CreateHtmlLabel(Label ?? Name ?? For.Name));
                    if(!string.IsNullOrEmpty(Icon) || Inline) html.Write(@$"<div class='input-group input-group-inline'>");
                    if(ShowLabel && Inline==true) html.Write(CreateHtmlLabel(Label ?? Name ?? For.Name));
                    
                    // section input
                    html.Write(input);                    


                    if(!string.IsNullOrEmpty(Icon)) {
                        
                        html.Write(@$"<label class=""mb-0 input-group-append"" for=""{Id}"">
                            <button type=""button"" class=""btn btn-default btn-sm px-1"" onclick=""$('#{Id}').not(':disabled').val('').trigger('change')""><i class=""bi bi-x-lg""></i></button>
                        </label>");
                        html.Write(@$"<label class=""mb-0 input-group-append"" for=""{Id}"">
                            <div class=""input-group-text px-1"" onclick=""$('#{Id}')[0]._flatpickr.toggle()"">
                                <i class=""{Icon}""></i>
                            </div>
                        </label>");
                    }
                    if(!string.IsNullOrEmpty(Icon) || Inline) html.Write("</div>");
                    html.Write(Createvalidation(For));
                    html.Write("</div>");
                    html.Write("</div>");
                    
                }else{
                    html.Write(@$"<div class='{(Type == "signature" ? "signature-pad-set" : "")} {Col}'>");
                    html.Write(@$"<div class='{(FormGroup == true ? "form-group" : "")}'>");
                    if(ShowLabel && Inline==false) html.Write(CreateHtmlLabel(Label ?? Name ?? For.Name));
                    if(!string.IsNullOrEmpty(Icon) || Inline) html.Write(@$"<div class='input-group input-group-inline'>");
                    if(ShowLabel && Inline==true) html.Write(CreateHtmlLabel(Label ?? Name ?? For.Name));
                    
                    // section input
                    html.Write(input);                    


                    if(!string.IsNullOrEmpty(Icon)) {
                        html.Write(@$"<label class=""mb-0 input-group-append"" for=""{Id}"">
                            <div class=""input-group-text"">
                                <i class=""{Icon}""></i>
                            </div>
                        </label>");
                    }
                    if(!string.IsNullOrEmpty(Icon) || Inline) html.Write("</div>");
                    html.Write(Createvalidation(For));
                    html.Write("</div>");
                    html.Write("</div>");
                }

                return html.ToString();
            }
        }

        private string Createvalidation(ModelExpression _For){
            TagBuilder validationMsg = null;
             
                validationMsg = _generator.GenerateValidationMessage(
                    ViewContext,
                    _For?.ModelExplorer,
                    _For?.Name ?? Name,
                    null,
                    ViewContext.ValidationMessageElement,
                    new { @class = "help-block text-danger" });
                validationMsg.Attributes["name"] = Name;
           
            return ConvertString(validationMsg);
        }

        private string CreateHtmlLabel (string _Label="") {
            var txtclass = "control-label";
            if(Inline) txtclass = "input-group-append form-item-inline";
            var txtRequired = "";
            if(Required) txtRequired = "<span class='ml-1 text-danger required-text'>*</span>";
            return @$"<label class=""{txtclass}"" for=""{Id}"">
                {_Label} {txtRequired} : 
            </label>";
        }

        private string extraAttribute(TagHelperContext _context){
            // ------------------- Begin Extra Attributes --------------------

            var attributeObjects = _context.AllAttributes.ToList();
            var props = this.GetType().GetProperties().Select(p => p.Name.Replace("_","").ToLower());
            attributeObjects.RemoveAll(a => props.Contains(a.Name.Replace("-","").ToLower()) || a.Name == "asp-for");

            var extraAttributeList = new List<string>();

            foreach (var attr in attributeObjects)
            {
                extraAttributeList.Add($"{attr.Name}=\"{attr.Value}\"");
            }                                                                                                 

            // ------------------- End Extra Attributes --------------------
            return string.Join(" ", extraAttributeList);
        }
    
        private string ConvertString(IHtmlContent content)
        {
            if(content != null){
                using (var writer = new StringWriter())
                {        
                    content.WriteTo(writer, HtmlEncoder.Default);
                    return writer.ToString();
                }
            }
            return "";
        }

        private string EncodeHtml(string mystring=""){
            return HtmlEncoder.Default.Encode(mystring);
        }
    }
}