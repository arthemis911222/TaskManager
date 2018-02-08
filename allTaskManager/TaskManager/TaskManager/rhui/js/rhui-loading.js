/**
 * Rhui Loading
 * @author  accountwcx@qq.com
 * @date    2015-07-29
 */

(function (window, $, Rhui, document) {
	'use strict';
	
	/**
	 * @public
	 * Loading实现等待效果
	 * @param  target  {String|jQuery|HTMLElement}
	 *                 Loading的目标元素，可以是HTML元素、jQuery实例、HTML元素id或者jQuery选择器
	 *                 如果匹配多个对象，只作用于第一个
	 *
	 * @param  opts    {String|Object} 初始化配置项
	 *             
	 *             options配置项：
	 *			   	   id         {String}  组件实例id，如果不指定则自动生成。
	 *			   	   message    {String}  显示内容，默认是[请稍候。。。]
	 *			   	   cls        {String}  css样式类
	 *			   	   style      {Object}  样式，会覆盖cls中的相同样式，默认是undefined
	 *                 events     {Object}  事件配置。Loading有onShow、onHide、onDestroy事件
	 *             
	 *             events配置项：
	 *                 onShow(this)       {Function}  显示事件，调用show方法后触发，默认是undefined
	 *                 onHide(this)       {Function}  隐藏事件，调用hide方法后触发，默认是undefined
	 *                 onDestroy(this)    {Function}  销毁事件，调用destroy方法后触发，默认是undefined
	 */
	var Loading = Rhui.component.Loading = function(target, opts){
		//获取Loading的实际目标对象
		var $target = Loading.initTarget(target);
		
		if(opts === null || opts === undefined){
			opts = {};
		}else if(Rhui.isString(opts)){
			opts = {message: opts};
		}
		
		var events = opts.events;
		//把defaults配置项和opts合并
		opts = $.extend({}, this.defaults, opts || {});
		opts.events = $.extend({}, opts.events, events || {});
		
		var old = $target.data(this.fullName);
		if(old && old instanceof Rhui.component.Loading){
			//如果$target已存在Loading，则返回
			old.setMessage(opts.message);
			return old;
		}
		
		this.$target = $target;
		this.options = opts;
		this.targetStatic = false;
		this.fixed = $target.get(0) === document.body;
		
		this._init(target);
		
		//管理当前组件
		//Rhui.addInstance(this);
	};
	
	//继承抽象组件
	Rhui.inherit(Loading, Rhui.component.AbstractComponent);
	
	//组件名称缩写，该名称将会注册到registerManager中
	Loading.cmpName = 'loading';
	Loading.prototype.name = Loading.cmpName;

	//全称，用于在$target中保存组件实例
	//如$target.data(Loading.prototype.fullName, Loading实例);
	Loading.fullName = Rhui.prefixName + 'loading';
	Loading.prototype.fullName = Loading.fullName;
	
	//类名
	Loading.className = 'Rhui.component.Loading';
	Loading.prototype.className = Loading.className;
	
	//注册Loading
	Rhui.register(Loading.cmpName, Loading);
	
	//默认配置项
	Loading.prototype.defaults = {
		//id: undefined,
		message: '请稍候。。。',
		//cls: undefined,
		//style: undefined,
		events: {
			//onShow: undefined,
			//onHide: undefined,
			//onDestroy: undefined
		}
	};
	
	/**
	 * @public
	 * @static
	 * 初始化目标对象
	 * @param   target  {String|jQuery|HTMLElement}
	 *                  Loading的目标元素，可以是HTML元素、jQuery实例、HTML元素id或者jQuery选择器。
	 *                  如果匹配多个对象，只返回第一个对象。
	 * @return  返回匹配的jQuery对象实例。
	 */
	Loading.initTarget = function(target){
		var $target;		
		if(target === window || target === undefined || target === null || target === document || target === document.body){
			$target = $('body');
		}else if(target.jquery || target instanceof $){
			if(target.length === 0 || target[0] === window || target[0] === document){
				$target = $('body');
			}else if(target.length === 1){
				$target = target;
			}else{
				$target = target.first();
			}
		}else{
			$target = $(target);
			if($target.length === 0){
				$target = $('body');
			}else if($target.length !== 1){
				$target = $target.first();
			}
		}
		
		return $target;
	};
	
	/**
	 * @private
	 * 初始化
	 */
	Loading.prototype._init = function(target){
		var $target = this.$target, opts = this.options;
		if($target.get(0) !== document.body && $target.css('position') === 'static'){
			$target.css('position', 'relative');
			this.targetStatic = true;
		}
		
		if(this.fixed){
			this.$overlay = $('<div class="rhui-overlay" style="position:fixed;"></div>');
			this.$self = $('<div class="rhui-loading" style="position:fixed;">' + opts.message + '</div>');
		}else{
			this.$overlay = $('<div class="rhui-overlay"></div>');
			this.$self = $('<div class="rhui-loading">' + opts.message + '</div>');
		}
		
		if(Rhui.isString(opts.cls)){
			this.$self.addClass(opts.cls);
		}
		
		if(Rhui.isObject(opts.style)){
			this.$self.css(opts.style);
		}
		
		this.$overlay.appendTo($target);
		this.$self.appendTo($target);
		
		//调用父类构造函数
		Rhui.component.AbstractComponent.call(this, target, opts);
		
		this.show();
		
		if(Rhui.isNumber(opts.width) && opts.width > 0){
			this.width = opts.width;
			this.$self.outerWidth(this.width, true);
		}else{
			this.width = this.$self.outerWidth(true);
		}
		
		if(Rhui.isNumber(opts.height) && opts.height > 0){
			this.height = opts.height;
			this.$self.outerHeight(this.height, true);
		}else{
			this.height = this.$self.outerHeight(true);
		}
		
		this.center();
		
		//$target.data(this.fullName, this);
	};
	
	/**
	 * @public
	 * 销毁组件，并触发onDestroy事件
	 * 支持链式调用
	 * @param   preventEvent  {Boolean}  [可选]true表示阻止onDestroy事件触发，留空或者false表示允许触发onDestroy事件
	 * @return  返回当前组件
	 */
	Loading.prototype.destroy = function(preventEvent){
		if(this.targetStatic){
			this.$target.css('position', 'static');
		}
		
		return this.baseDestroy(preventEvent);
	};
	
	/**
	 * @public
	 * 重置大小，并居中显示
	 * 支持链式调用
	 * @param   width         {Number}   [必填]宽度，单位像素
	 * @param   height        {Number}   [必填]高度，单位像素
	 * @param   preventEvent  {Boolean}  [可选]true表示阻止onResize事件触发，留空或者false表示允许触发onResize事件
	 * @return  返回当前组件
	 */
	Loading.prototype.resize = function(width, height, preventEvent){
		var self = this, $self = self.$self;
		if(Rhui.isNumber(width) && width > 0){
			self.width = width;
			$self.outerWidth(width, true);
		}
		
		if(Rhui.isNumber(height) && height > 1){
			self.height = height;
			$self.outerHeight(height, true);
		}
		
		self.center();
		
		if(!preventEvent && Rhui.isFunction(self.events.onResize)){
			self.events.onResize.call(self, self);
		}
		
		return self;
	};
	
	/**
	 * @public
	 * 居中显示
	 * 支持链式调用
	 * @return  返回当前组件
	 */
	Loading.prototype.center = function(){
		var $window,
			$target = this.$target,
			$self = this.$self;
		
		if(this.fixed){
			$window = $(window);
			$self.css({
				left: ($window.width() - this.width) / 2,
				top: ($window.height() - this.height) / 2
			});
		}else{
			$self.css({
				left: ($target.innerWidth() - this.width) / 2,
				top: ($target.innerHeight() - this.height) / 2
			});
		}
	};
	
	/**
	 * @public
	 * 设置Loading的内容，并重新定位
	 * 支持链式调用
	 * @param   message       {String}   更新的内容
	 * @param   preventEvent  {Boolean}  [可选]true表示阻止onResize事件触发，留空或者false表示允许触发onResize事件
	 * @return  当前组件
	 */
	Loading.prototype.setMessage = function(message, preventEvent){
		this.$self.html(message);
		this.autosize(preventEvent).center();
		return this;
	};
	
	/**
	 * @public
	 * 给Html元素添加Loading效果
	 * @param   target 
	 * @param   opts   {String|Object}
	 *                 [可选]Loading初始化配置参数。
	 *                 opts为空，使用默认配置添加Loading效果
	 *                 opts为String，opts为Loading的消息
	 * @return  返回创建的Loading
	 */
	Rhui.loading = function(target, opts){
		if(Rhui.isString(opts)){
			opts = {message: opts};
		}
		
		return new Loading(target, opts);
	};
	
	/**
	 * @public
	 * 取消Loading效果
	 * @param  target
	 */
	Rhui.unloading = function(target){
		var loading, $target = Loading.initTarget(target);
		if($target){
			loading = $target.data(Loading.fullName);
			if(loading){
				loading.destroy();
			}
		}
	};
})(window, $, window.Rhui, window.document);

