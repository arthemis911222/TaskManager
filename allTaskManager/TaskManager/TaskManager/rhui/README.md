#jQuery轻量级组件Rhui

Rhui是一个轻量级的jQuery组件集合，提供Button、Loading、Toolbar、Panel、Window和Dialog等一些常用的Web开发组件，支持IE7/8/9/10/11、Firefox和Chrome。

## Button

只要给html元素添加添加按钮类rhui-btn即可使用rhui提供的按钮样式。 按钮样式支持IE7/8/9/10/11、Firefox和Chrome浏览器，由于IE7/8不支持CSS3，所以按钮在IE7/8下没有圆角等CSS3效果。
按钮大小默认有4个级别 rhui-btn-large、 rhui-btn、 rhui-btn-small和 rhui-btn-min，其中 rhui-btn是默认级别。
按钮仿bootstrap提供几种常用颜色 rhui-btn-primary、 rhui-btn-success、 rhui-btn-info、 rhui-btn-warning和 rhui-btn-danger。

## Toolbar

Toolbar工具栏可通过Rhui.toolbar(target, options)和$('#id').rhui('toolbar', options)方法创建。
$('#toolbar1').rhui('toolbar', {
	width: 400,
	tools: [{
		name: 'btn1',
		text: '文件',
		click: function(toolbar, associate){
			alert('这里是按钮！')
		}
	}, {
		type: 'separator'
	}, {
		name: 'btn2',
		text: '复制',
		click: function(toolbar, associate){
			alert('这里是按钮2！')
		}
	}, {
		type: 'separator'
	}, {
		name: 'btn3',
		text: '删除',
		click: function(toolbar, associate){
			alert('这里是按钮2！')
		}
	}, {
		name: 'btn4',
		text: '导出',
		click: function(toolbar, associate){
			alert('这里是按钮2！')
		}
	}, {
		type: 'separator'
	}, {
		name: 'btn5',
		text: '查询',
		click: function(toolbar, associate){
			alert('这里是按钮2！')
		}
	}]
});

## Loading

Loading实现加载等待效果，支持整个页面或者指定元素的加载等待。给元素或者页面添加Loading效果有两种方法：Rhui.loading(target, options)和$('#id').rhui('loading', options)，取消Loading方法Rhui.unloading(target)。

## Panel

Panel面板可通过Html和CSS创建，也可以通过Rhui.panel(target, options)或者$('#id').rhui('panel', options)创建。

## Window

Window在Panel的基础上增加了浮动和拖动功能，可通过Rhui.widnow(target, options)或者$('#id').rhui('window', options)创建。

## Dialog

Rhui提供了alert、confirm和prompt三种Dialog对话框。