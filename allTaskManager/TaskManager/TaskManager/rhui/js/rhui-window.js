/**
 * Rhui Window
 * @author  accountwcx@qq.com
 * @date    2015-07-29
 */
 
(function (window, Rhui, $, document) {
	'use strict';
	
	/**
	 * @public
	 */
	var Window = Rhui.component.Window = function(target, opts){
		var $target = Window.initTarget(target);
		
		if(!$target){
			return null;
		}
		
		var old = $target.data(this.fullName);
		if(old && old instanceof Rhui.component.Window){
			//如果已存在，则返回
			return old;
		}
		
		if(!$target.hasClass('rhui-window')){
			$target.addClass('rhui-window');
		}
		
		if($target[0].parentNode !== document.body){
			$(document.body).append($target);
		}
		
		$target.children('.rhui-window-body').addClass('rhui-panel-body');
		
		if(opts === null || opts === undefined){
			opts = {};
		}
		
		var events = opts.events;
		//把defaults配置项和opts合并
		opts = $.extend({}, this.defaults, opts || {});
		opts.events = $.extend({}, opts.events, events || {});
		
		//把window移动到body下
		opts.renderTo = document.body;
		
		this._initFooter($target, opts);
		
		this.modal = false;
		if(opts.modal === true){
			this.modal = true;
			this.$overlay = $('<div class="rhui-overlay" style="position:fixed;"></div>');
			$('body').append(this.$overlay);
		}
		
		//调用父类构造函数
		Rhui.component.Panel.call(this, $target, opts);
		
		this.center();
		
		if(opts.autoShow){
			this.show();
		}else{
			this.hide();
		}
	};
	
	//继承Panel组件
	Rhui.inherit(Window, Rhui.component.Panel);
	
	Window.cmpName = 'window';
	Window.prototype.name = Window.cmpName;
	Window.fullName = Rhui.prefixName + 'window';
	Window.prototype.fullName = Window.fullName;
	Window.className = 'Rhui.component.Window';
	Window.prototype.className = Window.className;
	
	//注册Window
	Rhui.register(Window.cmpName, Window);
	
	Window.prototype.defaults = {
		autoShow: true,
		content: undefined, //String|jQuery|HTMLElement|Object {url: undefined, iframe: true}
		expanded: true,
		modal: true,
		hideFooter: false,
		cls: undefined,
		style: undefined,
		bodyCls: undefined,
		bodyStyle: undefined,
		title: '窗口',
		buttons: [{
			text: '关闭',
			cls: undefined,
			style: undefined,
			click: function(tb, win){
				win.hide();
			}
		}],
		closeAction: 'hide', //destroy, hide
		buttonAlign: 'center', //left,center,right
		isBodySize: false,
		width: 450,
		height: 300,
		headerTools: ['close'],
		events: {
			onInit: undefined,
			onExpand: undefined,
			onCollapse: undefined,
			onResize: undefined,
			onShow: undefined,
			onHide: undefined,
			onDestroy: undefined
		}
	};
	
	/**
	 * @public
	 * @static
	 * 初始化目标对象
	 * @param   target  {String|jQuery|HTMLElement}
	 *                  Window的目标元素，可以是HTML元素、jQuery实例、HTML元素id或者jQuery选择器。
	 *                  如果匹配多个对象，只返回第一个对象。
	 * @return  返回匹配的jQuery对象实例。
	 */
	Window.initTarget = function(target){
		var $target;
		if(target !== undefined && target !== null && target !== window && target !== document){
			if(target.jquery || target instanceof $){
				$target = target;
			}else if(Rhui.isString(target) || Rhui.isHTMLElement(target)){
				$target = $(target);
			}
		}
		
		if($target === undefined){
			if($target.length === 0){
				$target = undefined;
			}else if($target.length > 1){
				$target = $target.first();
			}
		}
		
		return $target;
	};
	
	/**
	 * @private
	 * 创建底部toolbar
	 *
	 */
	Window.prototype._initFooter = function($target, opts){
		var $footer = $target.children('.rhui-window-footer');
		if($footer.length === 0){
			$footer = $('<div class="rhui-window-footer"><div class="rhui-toolbar"></div></div>');
			$target.append($footer);
		}else if($footer.length !== 1){
			$footer = $footer.first();
		}
		
		var $toolbar = $footer.children('.rhui-toolbar');
		if($toolbar.length === 0){
			$toolbar = $('<div class="rhui-toolbar"></div>');
			$footer.append($toolbar);
		}else if($toolbar.length !== 1){
			$toolbar = $toolbar.first();
		}
		
		this.$footer = $footer;
		this.buttonBar = Rhui.toolbar($toolbar, {
			associate: this,
			tools: opts.buttons
		});
		
		this.$footer.css('text-align', opts.buttonAlign);
		this.buttonBar.$self.children('.rhui-toolbar-tool').removeClass('rhui-toolbar-tool');
	};
	
	/**
	 * @public
	 * 销毁窗口
	 * @param  preventEvent  {Boolean}  是否阻止触发onHide事件，true表示阻止，false或者留空表示不阻止。
	 */
	Window.prototype.destroy = function(preventEvent){
		var self = this;
		
		if(this.buttonBar){
			this.buttonBar.destroy();
			this.buttonBar = undefined;
		}
		
		if(this.$footer){
			this.$footer.remove();
			this.$footer = undefined;
		}
		
		if(this.$overlay){
			this.$overlay.remove();
			this.$overlay = undefined;
		}
		
		self.$title.remove();
		self.$title = undefined;
		
		self.headerToolbar.destroy();
		self.headerToolbar = undefined;
		
		self.$header.remove();
		self.$header = undefined;
		
		self.$body.remove();
		self.$body = undefined;
		
		this.baseDestroy(preventEvent);
	};
	
	/**
	 * @public
	 * 重新设置窗口的大小
	 * 支持链式调用
	 * @param  width         {Number}   [必填]宽度，单位像素
	 * @param  height        {Number}   [必填]高度，单位像素
	 * @param  preventEvent  {Boolean}  [可选]true表示阻止onResize事件触发，留空或者false表示允许触发onResize事件
	 * @return 返回当前Window
	 */
	Window.prototype.resize = function(width, height, preventEvent){
		var self = this,
			$self = this.$self,
			//innerWidth,
			headerHeight = this.hideHeader ? 0 : self.$header.outerHeight(true),
			footerHeight = this.hideFooter ? 0 : self.$footer.outerHeight(true);
			
		if(Rhui.isNumber(width) && width > 0){
			self.width = width;
			$self.outerWidth(width, true);
			
			//innerWidth = $self.width();
			//self.$body.outerWidth(innerWidth, true);
			//self.$header.outerWidth(innerWidth, true);
			//self.$footer.outerWidth(innerWidth, true);
		}
		
		if(Rhui.isNumber(height) && height > 1){
			self.height = height;
			$self.outerHeight(height, true);
			
			self.$body.outerHeight($self.height() - headerHeight - footerHeight);
		}
		
		if(!preventEvent && Rhui.isFunction(self.events.onResize)){
			self.events.onResize.call(self, self);
		}
		
		return this;
	};
	
	/**
	 * @public
	 * 设置Body的大小
	 * 支持链式调用
	 * @param   width         {Number}   [必填]宽度，单位像素
	 * @param   height        {Number}   [必填]高度，单位像素
	 * @param   preventEvent  {Boolean}  [可选]true表示阻止onResize事件触发，留空或者false表示允许触发onResize事件
	 * @return  返回当前Window
	 */
	Window.prototype.setBodySize = function(width, height, preventEvent){
		var self = this,
			$self = self.$self,
			headerHeight = this.hideHeader ? 0 : self.$header.outerHeight(true),
			footerHeight = this.hideFooter ? 0 : self.$footer.outerHeight(true);
		
		if(Rhui.isNumber(width) && width > 0){
			self.$body.outerWidth(width, true);
			//self.$header.outerWidth(width, true);
			$self.width(width);
		}
		
		if(Rhui.isNumber(height) && height > 1){
			self.$body.outerHeight(height, true);
			$self.height(height + headerHeight + footerHeight);
		}
		
		self.width = self.$self.outerWidth(true);
		self.height = self.$self.outerHeight(true);
		
		if(!preventEvent && Rhui.isFunction(self.events.onResize)){
			self.events.onResize.call(self, self);
		}
		
		return self;
	};
	
	/**
	 * @public
	 * 展开，并触发onExpand事件
	 * 支持链式调用
	 * @param   preventEvent  {Boolean}  true表示阻止触发onExpand，留空或者false表示触发onExpand事件
	 * @return  返回当前Window
	 */
	Window.prototype.expand = function(preventEvent){
		this.expanded = true;
		this.$body.show();
		
		var headerHeight = 0;
		if(!this.hideHeader){
			headerHeight = this.$header.outerHeight(true);
		}
		
		var footerHeight = 0;
		if(!this.hideFooter){
			this.$footer.show();
			footerHeight = this.$footer.outerHeight(true);
		}
		
		this.$self.height(headerHeight + this.$body.outerHeight(true) + footerHeight);
		this.$self.css('border-bottom-width', 1);
		
		if(preventEvent !== true && Rhui.isFunction(this.events.onExpand)){
			this.events.onExpand.call(this, this);
		}
		
		return this;
	};
	
	/**
	 * @public
	 * 收缩，并触发onCollapse事件
	 * 支持链式调用
	 * @param   preventEvent  {Boolean}  true表示阻止触发onCollapse，留空或者false表示触发onCollapse事件
	 * @return  返回当前Window
	 */
	Window.prototype.collapse = function(preventEvent){
		this.expanded = false;
		this.$body.hide();
		
		if(!this.hideFooter){
			this.$footer.hide();
		}
		
		this.$self.height(this.$header.outerHeight(true));
		this.$self.css('border-bottom-width', 0);
		
		if(preventEvent !== true && Rhui.isFunction(this.events.onCollapse)){
			this.events.onCollapse.call(this, this);
		}
		
		return this;
	};
	
	/**
	 * @public
	 * 让Window显示在页面中央
	 * 支持Window方法的链式调用
	 * @return 返回Window
	 */
	Window.prototype.center = function(){
		var self = this, $window = $(window);
		self.$self.css({
			top: ($window.height() - this.height) / 2,
			left: ($window.width() - this.width) / 2
		});
		return this;
	};
	
	/**
	 * @public
	 * 移动Window
	 * 支持Window方法的链式调用
	 * @param  x  {Number}  x坐标，单位像素
	 * @param  y  {Number}  y坐标，单位像素
	 * @return   返回当前Window
	 */
	Window.prototype.moveTo = function(x, y){
		if(!Rhui.isNumber(x)){
			x = 0;
		}
		
		if(!Rhui.isNumber(y)){
			y = 0;
		}
		
		this.$self.css({
			top: y,
			left: x
		});
		
		return this;
	};
	
	/**
	 * @public
	 * 设置窗口的模态效果
	 * 该方法支持链式调用
	 * @param  val  {Boolean}  留空或者true表示非模态，false表示模态
	 * @return  {Rhui.component.Window}
	 */
	Window.prototype.modeless = function(val){
		if(val === false){
			this.modal = true;
			this.$overlay.show();
		}else{
			this.modal = false;
			this.$overlay.hide();
		}
		
		return this;
	};
	
	/**
	 * 创建Window
	 * @param   target  {String|jQuery|HTMLElement}
	 *                  创建Panel的目标，可以是HTML元素、jQuery实例、HTML元素id或者jQuery选择器
	 *                  如果匹配多个对象，只作用于第一个
	 * @param   opts    {Object}  初始化配置项
	 * @return  返回当前Window
	 */
	Rhui.window = function(target, opts){
		return new Window(target, opts);
	};
})(window, window.Rhui, window.$, window.document);

(function(window, Rhui, $){
	function dialog(opts){
		opts = $.extend({}, {
			title: '提示',
			closeAction: 'destroy',
			buttonAlign: 'right',
			modal: true,
			width: 300,
			height: 150,
			bodyCls: 'rhui-dialog-body'
		}, opts || {});
		return Rhui.window('<div></div>', opts);
	}
	
	Rhui.alert = function(msg, title, handler){
		var argslen = arguments.length, opts;
		if(argslen === 1 && Rhui.isObject(msg)){
			return dialog(msg);
		}
		
		opts = {};
		opts.content = Rhui.isString(msg) ? msg : '';
		opts.title = Rhui.isString(title) ? title : '提示';		
		opts.buttons = [{
			text: '确定',
			cls: 'rhui-btn-primary',
			click: function(toolbar, dialog, event){
				if(Rhui.isFunction(handler)){
					handler.call(this, dialog, event);
				}
				dialog.destroy();
			}
		}];
		
		return dialog(opts);
	};
	
	Rhui.confirm = function(msg, title, handler){
		var argslen = arguments.length, opts;
		if(argslen === 1 && Rhui.isObject(msg)){
			return dialog(msg);
		}
		
		opts = {};
		opts.content = Rhui.isString(msg) ? msg : '';
		opts.title = Rhui.isString(title) ? title : '提示';
		opts.buttons = [{
			text: '确定',
			cls: 'rhui-btn-primary',
			click: function(toolbar, dialog, event){
				if(Rhui.isFunction(handler)){
					handler.call(this, true, dialog, event);
				}
				dialog.destroy();
			}
		},{
			text: '取消',
			click: function(toolbar, dialog, event){
				if(Rhui.isFunction(handler)){
					handler.call(this, false, dialog, event);
				}
				dialog.destroy();
			}
		}];
		
		return dialog(opts);
	};
	
	Rhui.prompt = function(msg, title, handler){
		var argslen = arguments.length, opts, template = '';
		if(argslen === 1 && Rhui.isObject(msg)){
			opts = msg;
			if(Rhui.isString(opts.msg)){
				template += '<div style="text-align:left;">' + opts.msg + '</div>';
			}
			template += '<div><input type="text" class="rhui-field" name="value" style="width:100%;" /></div>';
			opts.content = $(template);
			return dialog(opts);
		}
		
		opts = {};
		if(Rhui.isString(msg)){
			template += '<div style="text-align:left;">' + msg + '</div>';
		}
		template += '<div><input type="text" class="rhui-field" name="value" style="width:100%;" /></div>';
		
		opts.content = $(template);
		opts.title = Rhui.isString(title) ? title : '提示';
		opts.buttons = [{
			text: '确定',
			cls: 'rhui-btn-primary',
			click: function(toolbar, dialog, event){
				var val = $.trim(dialog.$body.find('[name="value"]').val());
				if(Rhui.isFunction(handler)){
					handler.call(this, true, val, dialog, event);
				}
				dialog.destroy();
			}
		},{
			text: '取消',
			click: function(toolbar, dialog, event){
				var val = $.trim(dialog.$body.find('[name="value"]').val());
				if(Rhui.isFunction(handler)){
					handler.call(this, false, val, dialog, event);
				}
				dialog.destroy();
			}
		}];
		
		return dialog(opts);
	};
})(window, window.Rhui, window.$);

