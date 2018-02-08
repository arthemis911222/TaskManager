/**
 * Rhui Toolbar
 * @author  accountwcx@qq.com
 * @date    2015-07-29
 */

(function(window, Rhui, $, document){
	'use strict';
	
	/**
	 * @public
	 * Toolbar
	 * @param  target  {String|jQuery|HTMLElement}
	 *                 创建Toolbar的目标，可以是HTML元素、jQuery实例、HTML元素id或者jQuery选择器
	 *                 如果匹配多个对象，只作用于第一个
	 *
	 * @param  opts    {Object} 初始化配置项
	 *             
	 *             options配置项：
	 *			   	   id         {String}     实例id，如果不指定则自动生成。
	 *                 associate  {Component}  toolbar的关联组件，该配置项在toolbar的按钮被点击时，会把associate当参数传过去，默认为undefined
	 *                 tools      {Array}      toolbar中的按钮
	 *                 width      {Number}     宽度，单位像素
	 *                 height     {Number}     高度，单位像素
	 *			   	   cls        {String}     css样式类
	 *			   	   style      {Object}     样式，会覆盖cls中的相同样式，默认是undefined
	 *                 events     {Object}     事件配置。Loading有onShow、onHide、onDestroy事件
	 *
	 *             tool配置项：
	 *                 name       {String}     tool名称，该名称唯一
	 *                 text       {String}     显示文本，可以是普通文本或者HTML标签内容
	 *                 cls        {String}     css样式类
	 *                 style      {Object}     样式，会覆盖cls中的相同样式，默认是undefined
	 *
	 *                 click(toolbar, associate)  {Function}
	 *                                         tool点击事件
	 *                                         参数toolbar为tool所在的toolbar组件
	 *                                         参数associate为toolbar的关联组件，如果没有关联组件则为空
	 *             
	 *             events配置项：
	 *                 onShow(this)       {Function}  显示事件，调用show方法后触发，默认是undefined
	 *                 onHide(this)       {Function}  隐藏事件，调用hide方法后触发，默认是undefined
	 *                 onDestroy(this)    {Function}  销毁事件，调用destroy方法后触发，默认是undefined
	 *
	 */
	var Toolbar = Rhui.component.Toolbar = function(target, opts){
		var self = this, $target = Toolbar.initTarget(target);
		
		if(!$target){
			return null;
		}
		
		//判断$target是否已经存在toolbar
		var old = $target.data(this.fullName);
		if(old && old instanceof Toolbar){
			//如果已存在，则返回
			return old;
		}
		
		if(opts === null || opts === undefined){
			opts = {};
		}
		
		this.$target = $target;
		this.$self = $target;
		this.associate = opts.associate;
		
		var events = opts.events;
		//把defaults配置项和opts合并
		opts = $.extend({}, this.defaults, opts || {});
		opts.events = $.extend({}, opts.events, events || {});
		this.options = opts;
		
		var $self = this.$self;
		if(!$self.hasClass('rhui-toolbar')){
			$self.addClass('rhui-toolbar');
		}
		
		this.$tools = [];
		this.addTools(opts.tools);
		
		//添加Css类
		if(Rhui.isString(opts.cls) && opts.cls !== ''){
			$self.addClass(opts.cls);
		}
		
		//添加样式
		if(Rhui.isObject(opts.style)){
			$self.css(opts.style);
		}
		
		//调用父类构造函数
		Rhui.component.AbstractComponent.call(this, target, opts);
		
		//重置大小
		self.resize(opts.width, opts.height, true);
		
		this.width = $self.outerWidth(true);
		this.height = $self.outerHeight(true);
		
		//$target.data(this.fullName, this);
		
		//管理当前组件
		//Rhui.addInstance(this);
	};
	
	//继承抽象组件
	Rhui.inherit(Toolbar, Rhui.component.AbstractComponent);
	
	//组件名称缩写，该名称将会注册到registerManager中
	Toolbar.cmpName = 'toolbar';
	Toolbar.prototype.name = Toolbar.cmpName;
	
	//全称，用于在$target中保存组件实例
	//如$target.data(Toolbar.prototype.fullName, Toolbar实例);
	Toolbar.fullName = Rhui.prefixName + 'toolbar';
	Toolbar.prototype.fullName = Toolbar.fullName;
	
	//类名
	Toolbar.className = 'Rhui.component.Toolbar';
	Toolbar.prototype.className = Toolbar.className;
	
	//注册Toolbar
	Rhui.register(Toolbar.cmpName, Toolbar);
	
	Toolbar.prototype.defaults = {
		//toolbar的关联组件，该配置项在toolbar的按钮被点击时，会把associate当参数传过去
		associate: undefined,
		tools: undefined,
		cls: undefined,
		style: undefined,
		width: undefined,
		height: undefined,
		events: {
			onInit: undefined,
			onShow: undefined,
			onHide: undefined,
			onDestroy: undefined,
			onResize: undefined
		}
	};
	
	/**
	 * @public
	 * @static
	 * 初始化目标对象
	 * @param   target  {String|jQuery|HTMLElement}
	 *                  Toolbar的目标元素，可以是HTML元素、jQuery实例、HTML元素id或者jQuery选择器。
	 *                  如果匹配多个对象，只返回第一个对象。
	 * @return  返回匹配的jQuery对象实例。
	 */
	Toolbar.initTarget = function(target){
		var $target;
		
		//对target进行判断
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
	 * @public
	 * 销毁组件，并触发onDestroy事件
	 * @param   preventEvent  {Boolean}  [可选]true表示阻止onDestroy事件触发，留空或者false表示允许触发onDestroy事件
	 */
	Toolbar.prototype.destroy = function(preventEvent){
		this.associate = undefined;
		$.each(this.$tools, function(i, v){
			v.remove();
		});
		this.$tools = undefined;
		
		this.baseDestroy(preventEvent);
	};
	
	/**
	 * @public
	 * 添加按钮
	 * 支持链式调用
	 * @param   tool  {String|Object}  按钮名称或者按钮配置项
	 * @return  返回当前toolbar
	 */
	Toolbar.prototype.addTool = function(tool){
		var self = this, $tool, type;
		tool = Rhui.isString(tool) ? Rhui.tools[tool] : tool;
		if(Rhui.isObject(tool)){
			type = tool.type;
			if(type === 'separator'){
				$tool = $('<div class="rhui-toolbar-tool rhui-toolbar-separator"></div>');
			}else{ //if(!Rhui.isString(type) || type === '' || type === 'button'){
				if(Rhui.isString(tool.name)){
					$tool = $('<div class="rhui-btn rhui-toolbar-tool" data-rhui-tool-name="' + tool.name + '">' + tool.text + '</div>');
				}else{
					$tool = $('<div class="rhui-btn rhui-toolbar-tool">' + tool.text + '</div>');
				}
			}
			
			if(Rhui.isObject(tool.style)){
				$tool.css(tool.style);
			}
		
			if(Rhui.isString(tool.cls) && tool.cls !== ''){
				$tool.addClass(tool.cls);
			}
			
			$tool.on('click', function(event){
				if(Rhui.isFunction(tool.click)){
					tool.click.call(this, self, self.associate, event);
				}
				return false;
			});
			
			self.$self.append($tool);
			self.$tools.push($tool);
		}
		
		return this;
	};
	
	/**
	 * @public
	 * 添加多个按钮
	 * 支持链式调用
	 * @param   tools  {String|Object}  按钮名称或者按钮配置项数组
	 * @return  返回当前toolbar
	 */
	Toolbar.prototype.addTools = function(tools){
		var i;
		if(Rhui.isArray(tools) && tools.length > 0){
			for(i = 0; i < tools.length; i++){
				this.addTool(tools[i]);
			}
		}
		
		return this;
	};
	
	/**
	 * @public
	 * 从toolbar中移除tool
	 * 支持链式调用
	 * @param   name  {String}  按钮名称
	 * @return  返回当前toolbar
	 */
	Toolbar.prototype.removeTool = function(name){
		var i, self = this, $tool, $tools = self.$tools;
		if(Rhui.isString(name) && name !== ''){
			for(i = 0; i < $tools.length; i++){
				$tool = $tools[i];
				if($tool.attr('data-rhui-tool-name') === name){
					$tool.remove();
					break;
				}
			}
			
			if(i < $tools.length){
				$tools.splice(i, 1);
			}
		}
	};
	
	/**
	 * @public
	 * 创建Toolbar
	 * @param   target
	 * @param   opts    {Object}  [可选]Toolbar初始化配置参数
	 * @return  {Rhui.component.Toolbar}  返回创建的Toolbar
	 */
	Rhui.toolbar = function(target, opts){
		return new Toolbar(target, opts);
	};
	
	Rhui.tools = {
		close: {
			name: 'close',
			type: 'button',
			text: '&times;',
			cls: undefined,
			style: {
				'font-size': '16px',
				'font-weight': 'bold'
			},
			click: function(toolbar, associate){
				if(!associate){
					return;
				}
				
				if(associate.closeAction === 'destroy'){
					associate.destroy();
				}else{
					associate.hide();
				}
			}
		},
		toggle: {
			name: 'toggle',
			type: 'button',
			text: 'toggle',
			cls: undefined,
			click: function(toolbar, associate){
				if(!associate || !Rhui.isFunction(associate.isExpanded)){
					return;
				}
				
				if(associate.isExpanded()){
					associate.expand();
				}else{
					associate.collapse();
				}
			}
		}
	};
})(window, window.Rhui, window.$, window.document);

