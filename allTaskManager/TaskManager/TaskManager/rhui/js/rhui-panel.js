/**
 * Rhui Panel
 * @author  accountwcx@qq.com
 * @date    2015-07-29
 */

(function(window, Rhui, $, document){
	'use strict';
	
	/**
	 * @public
	 * Panel
	 * @param  target  {String|jQuery|HTMLElement}
	 *                 创建Panel的目标，可以是HTML元素、jQuery实例、HTML元素id或者jQuery选择器
	 *                 如果匹配多个对象，只作用于第一个
	 *
	 * @param  opts    {Object} 初始化配置项
	 *             
	 *             options配置项：
	 *                 id         {String}     实例id，如果不指定则自动生成。
	 *                 associate  {Component}  toolbar的关联组件，该配置项在toolbar的按钮被点击时，会把associate当参数传过去，默认为undefined
	 *                 tools      {Array}      toolbar中的按钮
	 *                 width      {Number}     宽度，单位像素
	 *                 height     {Number}     高度，单位像素
	 *			   	   cls        {String}     css样式类
	 *			   	   style      {Object}     样式，会覆盖cls中的相同样式，默认是undefined
	 *                 renderTo   {String|jQuery|HTMLElement}
	 *                                         Panel显示的目标元素
	 *                 events     {Object}     事件配置。Loading有onShow、onHide、onDestroy事件
	 *                                         参数associate为toolbar的关联组件，如果没有关联组件则为空
	 *             
	 *             events配置项：
	 *                 onShow(this)       {Function}  显示事件，调用show方法后触发，默认是undefined
	 *                 onHide(this)       {Function}  隐藏事件，调用hide方法后触发，默认是undefined
	 *                 onDestroy(this)    {Function}  销毁事件，调用destroy方法后触发，默认是undefined
	 *
	 */
	var Panel = Rhui.component.Panel = function(target, opts){
		var self = this, $target = Panel.initTarget(target);
		
		if(!$target){
			return null;
		}
		
		var old = $target.data(this.fullName);
		if(old && old instanceof Rhui.component.Panel){
			//如果已存在，则返回
			return old;
		}
		
		this.$target = $target;
		this.$self = $target;
		
		if(opts === null || opts === undefined){
			opts = {};
		}
		
		var events = opts.events;
		//把defaults配置项和opts合并
		opts = $.extend({}, this.defaults, opts || {});
		opts.events = $.extend({}, opts.events, events || {});
		this.options = opts;
		
		this.expanded = opts.expanded;
		
		var $self = this.$self;
		if(!$self.hasClass('rhui-panel')){
			$self.addClass('rhui-panel');
		}
		
		this.renderTo(opts.renderTo);
		
		this.$header = $self.children('.rhui-panel-header');
		if(this.$header.length === 0){
			this.$header = $('<div class="rhui-panel-header"><div class="rhui-panel-header-title"></div><div class="rhui-toolbar"></div></div>');
			$self.prepend(this.$header);
		}
		
		this.$title = this.$header.children('.rhui-panel-header-title');
		if(this.$title.length === 0){
			this.$title = $('<div class="rhui-panel-header-title"></div>');
			this.$header.prepend(this.$title);
		}
		
		this._initHeaderToolbar();
		
		this.$body = $self.children('.rhui-panel-body');
		if(this.$body.length === 0){
			this.$body = $('<div class="rhui-panel-body"></div>');
			this.$body.insertAfter(this.$header);
		}
		
		if(Rhui.isString(opts.title)){
			this.$title.html(opts.title);
			this.title = opts.title;
		}
		
		//给Panel的body区域添加Css类
		if(Rhui.isString(opts.bodyCls) && opts.bodyCls !== ''){
			this.$body.addClass(opts.bodyCls);
		}
		
		//给Panel的body区域添加样式
		if(Rhui.isObject(opts.bodyStyle)){
			this.$body.css(opts.bodyStyle);
		}
		
		if(opts.content !== undefined && opts.content !== null){
			if(Rhui.isString(opts.content) || opts.content.jquery || opts.content instanceof $){
				this.$body.html(opts.content);
			}else if(Rhui.isObject(opts.content)){
				if(Rhui.isString(opts.content.url)){
					if(opts.content.iframe === true){
						this.loadIframe(opts.content.url);
					}else{
						this.load(opts.content.url);
					}
				}
			}
		}
		
		//给Panel的添加Css类
		if(Rhui.isString(opts.cls) && opts.cls !== ''){
			$self.addClass(opts.cls);
		}
		
		//给Panel添加样式
		if(Rhui.isObject(opts.style)){
			$self.css(opts.style);
		}
		
		//设置Panel的大小
		if(opts.isBodySize === true){
			self.setBodySize(opts.width, opts.height, true);
		}else{
			self.resize(opts.width, opts.height, true);
		}
		
		//调用父类构造函数
		Rhui.component.AbstractComponent.call(this, target, opts);
		
		//$target.data(this.fullName, this);
		
		//管理当前组件
		//Rhui.addInstance(this);
	};
	
	//继承抽象组件
	Rhui.inherit(Panel, Rhui.component.AbstractComponent);
	
	Panel.cmpName = 'panel';
	Panel.prototype.name = Panel.cmpName;
	Panel.fullName = Rhui.prefixName + 'panel';
	Panel.prototype.fullName = Panel.fullName;
	Panel.className = 'Rhui.component.Panel';
	Panel.prototype.className = Panel.className;
	
	//注册Panel
	Rhui.register(Panel.cmpName, Panel);
	
	Panel.prototype.defaults = {
		content: undefined, //String|jQuery|HTMLElement|Object {url: undefined, iframe: true}
		renderTo: undefined, //HTML元素id|jQuery|HTMLElement
		expanded: true,
		cls: undefined,
		style: undefined,
		bodyCls: undefined,
		bodyStyle: undefined,
		closAction: 'hide',
		title: undefined,
		isBodySize: false,
		width: undefined,
		height: undefined,
		headerTools: [],
		events: {
			onInit: undefined,
			onExpand: undefined,
			onCollapse: undefined,
			onShow: undefined,
			onHide: undefined,
			onResize: undefined,
			onDestroy: undefined
		}
	};
	
	/**
	 * @public
	 * @static
	 * 初始化目标对象
	 * @param   target  {String|jQuery|HTMLElement}
	 *                  Panel的目标元素，可以是HTML元素、jQuery实例、HTML元素id或者jQuery选择器。
	 *                  如果匹配多个对象，只返回第一个对象。
	 * @return  返回匹配的jQuery对象实例。
	 */
	Panel.initTarget = function(target){
		var $target;
		
		if(target !== undefined && target !== null && target !== window && target !== document){
			if(target.jquery || target instanceof $){
				$target = target;
			}else if(Rhui.isString(target) || Rhui.isHTMLElement(target)){
				$target = $(target);
			}
		}
		
		if($target !== undefined){
			if($target.length > 1){
				$target = $target.first();
			}else if($target.length === 0){
				$target = undefined;
			}
		}
		
		return $target;
	};
	
	/**
	 * @private
	 * 创建Header Toolbar
	 *
	 */
	Panel.prototype._initHeaderToolbar = function(){
		var $headerToolbar = this.$header.children('.rhui-toolbar');
		if($headerToolbar.length === 0){
			$headerToolbar = $('<div class="rhui-toolbar"></div>');
			this.$header.append($headerToolbar);
		}
		
		this.headerToolbar = Rhui.toolbar($headerToolbar, {
			associate: this,
			tools: this.options.headerTools
		});
	};
	
	/**
	 * @public
	 * 获取标题
	 * @return  {String}  标题
	 */
	Panel.prototype.getTitle = function(){
		return this.title;
	};
	
	/**
	 * @public
	 * 设置标题
	 * 支持链式调用
	 * @param   {String}  标题
	 * @return  当前面板
	 */
	Panel.prototype.setTitle = function(title){
		this.title = title;
		this.$title.html(title);
		return this;
	};
	
	/**
	 * @public
	 * 返回Body
	 * @return {jQuery}
	 */
	Panel.prototype.getBody = function(){
		return this.$body;
	};
	
	/**
	 * @public
	 * 重置Panel的大小
	 * 支持链式调用
	 * @param   width         {Number}   [必填]宽度，单位像素
	 * @param   height        {Number}   [必填]高度，单位像素
	 * @param   preventEvent  {Boolean}  [可选]true表示阻止onResize事件触发，留空或者false表示允许触发onResize事件
	 * @return  返回当前Panel
	 */
	Panel.prototype.resize = function(width, height, preventEvent){
		var self = this, $self = self.$self;
		if(Rhui.isNumber(width) && width > 0){
			self.width = width;
			$self.outerWidth(width, true);
			
			//var innerWidth = $self.width();
			//self.$body.outerWidth(innerWidth, true);
			//self.$header.outerWidth(innerWidth, true);
		}
		
		if(Rhui.isNumber(height) && height > 1){
			self.height = height;
			$self.outerHeight(height, true);
			
			self.$body.outerHeight($self.height() - self.$header.outerHeight(true));
		}
		
		if(!preventEvent && Rhui.isFunction(self.events.onResize)){
			self.events.onResize.call(self, self);
		}
		
		return this;
	};
	
	/**
	 * @public
	 * 设置Panel Body的大小
	 * 支持链式调用
	 * @param   width         {Number}   [必填]宽度，单位像素
	 * @param   height        {Number}   [必填]高度，单位像素
	 * @param   preventEvent  {Boolean}  [可选]true表示阻止onResize事件触发，留空或者false表示允许触发onResize事件
	 * @return  返回当前Panel
	 */
	Panel.prototype.setBodySize = function(width, height, preventEvent){
		var self = this, $self = self.$self;
		
		if(Rhui.isNumber(width) && width > 0){
			self.$body.outerWidth(width, true);
			//self.$header.outerWidth(width, true);
			$self.width(width);
		}
		
		if(Rhui.isNumber(height) && height > 1){
			self.$body.outerHeight(height, true);
			$self.height(height + self.$header.outerHeight(true));
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
	 * 判断Panel是否展开
	 * @return  展开返回true，否则返回false
	 */
	Panel.prototype.isExpanded = function(){
		return this.expanded;
	};
	
	/**
	 * @public
	 * 展开Panel，并触发onExpand事件
	 * 支持链式调用
	 * @param   preventEvent  {Boolean}  true表示阻止触发onExpand，留空或者false表示触发onExpand事件
	 * @return  返回当前Panel
	 */
	Panel.prototype.expand = function(preventEvent){
		this.expanded = true;
		this.$body.show();
		
		this.$self.height(this.$header.outerHeight(true) + this.$body.outerHeight(true));
		this.$self.css('border-bottom-width', 1);
		
		if(preventEvent !== true && Rhui.isFunction(this.events.onExpand)){
			this.events.onExpand.call(this, this);
		}
		
		return this;
	};
	
	/**
	 * @public
	 * 收缩Panel，并触发onCollapse事件
	 * 支持链式调用
	 * @param   preventEvent  {Boolean}  true表示阻止触发onCollapse，留空或者false表示触发onCollapse事件
	 * @return  返回当前Panel
	 */
	Panel.prototype.collapse = function(preventEvent){
		this.expanded = false;
		this.$body.hide();
		
		this.$self.height(this.$header.outerHeight(true));
		this.$self.css('border-bottom-width', 0);
		
		if(preventEvent !== true && Rhui.isFunction(this.events.onCollapse)){
			this.events.onCollapse.call(this, this);
		}
		
		return this;
	};
	
	/**
	 * @public
	 * 通过ajax方式加载Panel内容
	 * 支持链式调用
	 * @param   url      {String}    路径
	 * @param   data     {Object}    请求参数，如果data数据不为空，则发送post请求
	 * @param   done     {Function}  成功后回调函数
	 * @param   fail     {Function}  失败后回调函数
	 * @param   timeout  {Number}    请求超时，单位毫秒，默认60000毫秒
	 * @return  当前Panel
	 */
	Panel.prototype.load = function(url, data, done, fail, timeout){
		var self = this,
			$body = self.$body,
			opts = {};
			
		$body.rhui('loading', '加载中。。。');
		
		opts.url = url;
		opts.success = function(data){
			$body.rhui('loading').destroy();
			$body.html(data);
			if(Rhui.isFunction(done)){
				done.call(self, self, data);
			}
		};
		opts.failure = function(){
			$body.rhui('loading').destroy();
			if(Rhui.isFunction(fail)){
				fail.call(self, self);
			}
		};
		
		if(Rhui.isNumber(timeout) && timeout > 15000){
			opts.timeout = timeout;
		}
		
		if(Rhui.isObject(data)){
			opts.data = data;
			opts.type = 'post';
		}
		
		$.ajax(opts);
		
		return self;
	};
	
	/**
	 * @public
	 * 通过iframe方式加载Panel内容
	 * 支持链式调用
	 * @param   url  {String}  iframe的路径
	 * @return  当前Panel
	 */
	Panel.prototype.loadIframe = function(url){
		var self = this, $body = self.$body, $iframe = $body.children('iframe');
		if($iframe.length === 0){
			$body.children().each(function(){
				this.remove();
			});
			$body.html('<iframe frameborder="0" src="' + url + '" style="width:100%;height:100%;margin:0;padding:0;border-width:0;outline-width:0;"></iframe>');
		}else{
			$iframe.attr('src', url);
		}
		
		return self;
	};
	
	/**
	 * @public
	 * 销毁Panel，并触发onDestroy事件
	 * @param   preventEvent  {Boolean}  [可选]true表示阻止onDestroy事件触发，留空或者false表示允许触发onDestroy事件
	 */
	Panel.prototype.destroy = function(preventEvent){
		var self = this;
		
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
	
	Rhui.panel = function(target, opts){
		return new Panel(target, opts);
	};
})(window, window.Rhui, window.$, window.document);

