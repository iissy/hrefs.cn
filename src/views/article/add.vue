<template>
    <div style="position: relative;">
        <Menu tagIndex="4"></Menu>
        <div class="rightMain">
            <Header></Header>
            <div id="list">
                <form class="form-horizontal mg0">
                    <div id="dataform" class="row">
                        <div class="col-md-12">
                            <div class="portlet light" style="margin-bottom: 0;">
                                <div class="portlet-body form" style="margin:0 auto;">
                                    <div class="form-horizontal mg0" role="form">
                                        <div class="form-body" style="padding:0 0 0 0;">
                                            <div class="form-group">
                                                <div class="col-md-12">
                                                    <input type="text" v-model="Title" class="form-control" placeholder="请在此填写文章标题" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-md-12">
                                                    <input type="text" id="Id" v-model="Id" class="form-control" placeholder="请在此填写文章 url 链接" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-md-12">
                                                    <div id="imgfileinput" data-provides="fileinput" class="fileinput fileinput-new right">
                                                        <span class="btn green btn-file">
                                                            <span class="fileinput-new"> 选择图片 </span>
                                                            <input type="file" id="upload" @change="change" name="upload" />
                                                        </span>
                                                    </div>
                                                    <input type="text" v-model="Icon" class="form-control" placeholder="上传图片" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-md-12">
                                                    <ckeditor :editor="editor" v-model="Body" :config="editorConfig" @ready="onEditorReady"></ckeditor>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-md-1 control-label"></label>
                                                <div class="col-md-11">
                                                    <button style="color:#ffffff;padding:5px 50px 5px 50px;background-color: #36c6d3;font-size:16px;border-radius:10px;border: 1px solid #2bb8c4;float:right;" type="button" v-on:click="post">发 布</button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </form>
            </div>
        </div>
    </div>
</template>

<script>
    import Header from '../../components/header';
    import Menu from '../../components/menu';
    import Vue from 'vue';
    import CKEditor from '@ckeditor/ckeditor5-vue';
    import ClassicEditor from '@ckeditor/ckeditor5-editor-classic/src/classiceditor';
    import EssentialsPlugin from '@ckeditor/ckeditor5-essentials/src/essentials';
    import BoldPlugin from '@ckeditor/ckeditor5-basic-styles/src/bold';
    import ItalicPlugin from '@ckeditor/ckeditor5-basic-styles/src/italic';
    import LinkPlugin from '@ckeditor/ckeditor5-link/src/link';
    import ParagraphPlugin from '@ckeditor/ckeditor5-paragraph/src/paragraph';

    import Underline from '@ckeditor/ckeditor5-basic-styles/src/underline';
    import Strikethrough from '@ckeditor/ckeditor5-basic-styles/src/strikethrough';
    import Code from '@ckeditor/ckeditor5-basic-styles/src/code';
    import CodeBlock from '@ckeditor/ckeditor5-code-block/src/codeblock';
    import Subscript from '@ckeditor/ckeditor5-basic-styles/src/subscript';
    import Superscript from '@ckeditor/ckeditor5-basic-styles/src/superscript';

    import Indent from '@ckeditor/ckeditor5-indent/src/indent';
    import IndentBlock from '@ckeditor/ckeditor5-indent/src/indentblock';

    import Font from '@ckeditor/ckeditor5-font/src/font';
    import FontFamily from '@ckeditor/ckeditor5-font/src/fontfamily';

    import CKFinder from '@ckeditor/ckeditor5-ckfinder/src/ckfinder';
    import ImageUpload from '@ckeditor/ckeditor5-image/src/imageupload';

    import Image from '@ckeditor/ckeditor5-image/src/image';
    import ImageToolbar from '@ckeditor/ckeditor5-image/src/imagetoolbar';
    import ImageCaption from '@ckeditor/ckeditor5-image/src/imagecaption';
    import ImageStyle from '@ckeditor/ckeditor5-image/src/imagestyle';
    import ImageResize from '@ckeditor/ckeditor5-image/src/imageresize';

    import Heading from '@ckeditor/ckeditor5-heading/src/heading';

    import Table from '@ckeditor/ckeditor5-table/src/table';
    import TableToolbar from '@ckeditor/ckeditor5-table/src/tabletoolbar';
    import TableProperties from '@ckeditor/ckeditor5-table/src/tableproperties';
    import TableCellProperties from '@ckeditor/ckeditor5-table/src/tablecellproperties';

    import Alignment from '@ckeditor/ckeditor5-alignment/src/alignment';

    import HorizontalLine from '@ckeditor/ckeditor5-horizontal-line/src/horizontalline';

    import List from '@ckeditor/ckeditor5-list/src/list';
    import TodoList from '@ckeditor/ckeditor5-list/src/todolist';
    import router from '../../router';
    import httper from '../../util/httper';
    import '../../util/upload';
    import server from "../../conf/config";
    import $ from 'jquery';

    Vue.use(CKEditor);
    export default {
        data: function () {
            return {
                Id: '',
                Title: '',
                Icon: '',
                Brief: '',
                Body: '',
                editor: ClassicEditor,
                editorConfig: {
                    plugins: [
                        EssentialsPlugin,
                        BoldPlugin,
                        ItalicPlugin,
                        LinkPlugin, HorizontalLine,
                        ParagraphPlugin, Underline, Strikethrough, Subscript, Superscript, Indent, IndentBlock, TodoList,
                        Code, CodeBlock, Font, FontFamily, List, CKFinder, ImageUpload,
                        Image, ImageToolbar, ImageCaption, ImageStyle, ImageResize,
                        Heading, Alignment,
                        Table, TableToolbar, TableProperties, TableCellProperties
                    ],
                    toolbar: {
                        items: [
                            'heading',
                            'bold',
                            'italic',
                            'fontSize', 'fontFamily', 'fontColor', 'fontBackgroundColor',
                            'underline', 'strikethrough', 'subscript', 'superscript',
                            'link',
                            'undo', 'redo', 'outdent', 'indent',
                            'code', 'codeBlock',
                            'alignment',
                            'horizontalLine',
                            'bulletedList', 'numberedList', 'todoList',
                            'imageUpload',
                            'insertTable'
                        ]
                    },
                    indentBlock: {
                        offset: 1,
                        unit: 'em'
                    },
                    image: {
                        toolbar: ['imageTextAlternative', '|', 'imageStyle:alignLeft', 'imageStyle:full', 'imageStyle:alignRight'],
                        styles: ['full', 'alignLeft', 'alignRight']
                    },
                    alignment: {
                        options: [ 'left', 'center', 'right' ]
                    },
                    table: {
                        contentToolbar: [
                            'tableColumn', 'tableRow', 'mergeTableCells', 'tableProperties', 'tableCellProperties'
                        ],
                        tableProperties: {},
                        tableCellProperties: {}
                    },
                    codeBlock: {
                        languages: [
                            { language: 'plaintext', label: 'Plain text' },
                            { language: 'go', label: 'Golang' },
                            { language: 'cs', label: 'C#' },
                            { language: 'cpp', label: 'C++' },
                            { language: 'css', label: 'CSS' },
                            { language: 'sql', label: 'SQL' },
                            { language: 'xml', label: 'HTML/XML' },
                            { language: 'java', label: 'Java' },
                            { language: 'javascript', label: 'JavaScript' },
                            { language: 'php', label: 'PHP' },
                            { language: 'python', label: 'Python' },
                            { language: 'ruby', label: 'Ruby' },
                            { language: 'typescript', label: 'TypeScript' },
                            { language: 'c', label: 'C' }
                        ]
                    },
                    ckfinder: {
                        uploadUrl: server.upload,
                        options: {
                            resourceType: 'Images'
                        }
                    }
                }
            };
        },
        components: {
            Header,
            Menu,
            ckeditor: CKEditor.component
        },
        created: function () {
            let self = this;
            if (self.$route.params.id) {
                let url = '/api/article/get/' + self.$route.params.id;
                httper.get(url).then(function (response) {
                    self.Id = response.data.id;
                    self.Title = response.data.title;
                    self.Icon = response.data.icon;
                    self.Brief = response.data.brief;
                    self.Body = response.data.body;
                    $("#Id").attr("disabled", true);
                });
            }
        },
        methods: {
            onEditorReady(editor) {
                this.element(editor, 'div')
                this.element(editor, 'pre')
            },
            element: function (editor, ele) {
                editor.model.schema.register(ele, {
                    allowWhere: '$block',
                    allowContentOf: '$root'
                });

                // Allow <div> elements in the model to have all attributes.
                editor.model.schema.addAttributeCheck(context => {
                    if (context.endsWith(ele)) {
                        return true;
                    }
                });

                // View-to-model converter converting a view <div> with all its attributes to the model.
                editor.conversion.for('upcast').elementToElement({
                    view: ele,
                    model: (viewElement, modelWriter) => {
                        return modelWriter.createElement(ele, viewElement.getAttributes());
                    }
                });

                // Model-to-view converter for the <div> element (attributes are converted separately).
                editor.conversion.for('downcast').elementToElement({
                    model: ele,
                    view: ele
                });

                // Model-to-view converter for <div> attributes.
                // Note that a lower-level, event-based API is used here.
                editor.conversion.for('downcast').add(dispatcher => {
                    dispatcher.on('attribute', (evt, data, conversionApi) => {
                        // Convert <div> attributes only.
                        if (data.item.name !== ele) {
                            return;
                        }

                        const viewWriter = conversionApi.writer;
                        const viewDiv = conversionApi.mapper.toViewElement(data.item);

                        // In the model-to-view conversion we convert changes.
                        // An attribute can be added or removed or changed.
                        // The below code handles all 3 cases.
                        if (data.attributeNewValue) {
                            viewWriter.setAttribute(data.attributeKey, data.attributeNewValue, viewDiv);
                        } else {
                            viewWriter.removeAttribute(data.attributeKey, viewDiv);
                        }
                    });
                });
            },
            change: function () {
                let self = this;
                $("#upload").upload(server.upload, function (response) {
                    self.Icon = response.url;
                });
            },
            post: function () {
                let self = this;
                if (!!$.trim(self.Icon) && !!$.trim(self.Id) && !!$.trim(self.Title) && !!$.trim(self.Body)) {
                    httper.post('/api/article/save', {
                        id: self.Id,
                        title: self.Title,
                        icon: self.Icon,
                        brief: self.Brief,
                        body: self.Body
                    }).then(function (response) {
                        if (response.data > 0) {
                            router.push({name: 'ArticleList', params: {size: 14, page: 1}});
                        }
                    });
                }
            }
        }
    };
</script>