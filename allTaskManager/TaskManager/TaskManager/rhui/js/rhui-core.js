/**
 * Rhui Core
 * @author  accountwcx@qq.com
 * @date    2015-08-12
 */

(function(window, $){
	'use strict';
	
	var Rhui = window.Rhui || {};
	window.Rhui = Rhui;
	
	Rhui.isString = function (val) {
		return Object.prototype.toString.call(val) === '[object String]';
	};

	Rhui.isNullOrUndefined = function (val) {
		return (val === null) || (val === undefined);
	};
	
	Rhui.isFunction = function (val) {
		return Object.prototype.toString.call(val) === '[object Function]';
	};
	
	Rhui.isObject = function (val) {
		return val !== null && val !== undefined && Object.prototype.toString.call(val) === '[object Object]';
	};
	
	Rhui.isArray = function (val) {
		return Object.prototype.toString.call(val) === '[object Array]';
	};
	
	Rhui.isNumber = function (val) {
		return Object.prototype.toString.call(val) === '[object Number]';
	};
	
	Rhui.isDate = function (val) {
		return Object.prototype.toString.call(val) === '[object Date]';
	};
	
	Rhui.isBoolean = function (val) {
		return Object.prototype.toString.call(val) === '[object Boolean]';
	};
	
	Rhui.isHTMLElement = function(val){
		if(val && val.nodeType === 1){
			return true;
		}else{
			return false;
		}
	};
	
	//判断浏览器类型以及版本号
	(function(){
		var ua = window.navigator.userAgent.toLowerCase(),
			browser = {},
			check;
			
		if(check = ua.match(/msie ([\d.]+)/)){
			browser.name = 'msie';
			browser.version = check[1];  
		}else if(check = ua.match(/trident 7.0/)){
			browser.version = 'msie';
			browser.ie = '11.0';  
		}else if(check = ua.match(/chrome\/([\d.\d]+)/)){
			browser.name = 'chrome';
			browser.version = check[1];  
		}else if(check = ua.match(/firefox\/([\d.\d]+)/)){
			browser.name = 'firefox';
			browser.version = check[1];
		}else if(check = /(opera)(?:.*version|)[ \/]([\w.]+)/.exec(ua)){  
			browser.name = 'opera';
			browser.version = check[2];
		}
		
		Rhui.browser = browser;
		Rhui.isIE = Rhui.browser.name === 'msie';
		Rhui.isChrome = Rhui.browser.name === 'chrome';
		Rhui.isOpera = Rhui.browser.name === 'opera';
		Rhui.isFirefox = Rhui.browser.name === 'mozilla';
	})();
	
	/**
	* 生成uuid
	*/
	Rhui.uuid = function(){
		var chars = '0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz'.split(''),
			uuid = [], rnd = 0, r, i;
		
		for (i = 0; i < 36; i++) {
			if(i===8 || i===13 || i===18 || i===23){
				uuid[i] = '-';
			}else if(i===14){
				uuid[i] = '4';
			}else{
				if(rnd <= 0x02){
					rnd = 0x2000000 + (Math.random()*0x1000000)|0;
				}
				r = rnd & 0xf;
				rnd = rnd >> 4;
				uuid[i] = chars[(i == 19) ? (r & 0x3) | 0x8 : r];
			}
		}
		
		return uuid.join('');
	};
	
	/**
	* 常用的字符串处理方法
	* @author accountwcx@qq.com
	* @date   2015-08-12
	*/
	Rhui.String = (function(){
		// 字符串去掉两边空白正则表达式
		var trimRegex = /^[\x09\x0a\x0b\x0c\x0d\x20\xa0\u1680\u180e\u2000\u2001\u2002\u2003\u2004\u2005\u2006\u2007\u2008\u2009\u200a\u2028\u2029\u202f\u205f\u3000]+|[\x09\x0a\x0b\x0c\x0d\x20\xa0\u1680\u180e\u2000\u2001\u2002\u2003\u2004\u2005\u2006\u2007\u2008\u2009\u200a\u2028\u2029\u202f\u205f\u3000]+$/g,
			
			// 字符串格式化正则表达式
			formatRegex = /\{(\d+)\}/g,
			
			htmlEncodeMap = {
				'"': "&quot;",
				'&': "&amp;",
				"'": "&#39;",
				'<': "&lt;",
				'>': "&gt;"
			},
			
			htmlDecodeMap = {
				'&#39;': "'",
				'&amp;': "&",
				'&gt;': ">",
				'&lt;': "<",
				'&quot;': '"'
			},
			
			htmlEncodeRegex = /(&|>|<|"|')/g,
			
			htmlDecodeRegex = /(&amp;|&gt;|&lt;|&quot;|&#39;|&#[0-9]{1,5};)/g;
	
		return {
			/**
			* 除去字符串两端的空白字符
			* @param   str  {String}  字符串
			* @return  返回处理后的字符串
			*/
			trim: function(str){
				return str.replace(trimRegex, '');
			},
			
			/**
			* 允许用格式化的方式给传值
			* var cls = 'css-class', text = '内容';
			* var str = Rhui.String.format('<div class="{0}">{1}</div>', cls, text);
			* // str的内容是 <div class="css-class">内容</div>
			*
			* @param   str        {String}  待格式化的字符串
			* @param   params...  {Object}  与字符串中{0}、{1}...匹配的内容
			* @return  返回格式化后的字符串
			*/
			format: function(str) {
				var i, args = arguments, len = args.length, arr = [];
				for(i = 1; i < len; i++){
					arr.push(args[i]);
				}
				
				return str.replace(formatRegex, function(m, i) {
					return arr[i];
				});
			},
			
			/**
			* 填补字符串左边
			* @param   str        {String}  原字符串
			* @param   size       {Number}  填补后的长度
			* @param   character  {String}  填补的字符，如果不填则为空字符' '
			* @return  返回填补后的字符串
			*/
			leftPad: function(str, size, character) {
				var result = '' + str;
				
				if(Object.prototype.toString.call(character) !== '[object String]'){
					character = ' ';
				}
				
				while (result.length < size) {
					result = character + result;
				}
				
				return result;
			},
			
			/**
			* 填补字符串右边
			* @param   str        {String}  原字符串
			* @param   size       {Number}  填补后的长度
			* @param   character  {String}  填补的字符，如果不填则为空字符' '
			* @return  返回填补后的字符串
			*/
			rightPad: function(str, size, character) {
				var result = '' + str;
				
				if(Object.prototype.toString.call(character) !== '[object String]'){
					character = ' ';
				}
				
				while (result.length < size) {
					result += character;
				}
				
				return result;
			},
			
			/**
			* 把字符串中的html字符转义
			* @param   str  {String}
			* @return  返回转义后的字符
			*/
			htmlEncode: function(str) {
				if(Object.prototype.toString.call(str) === '[object String]'){
					return str.replace(htmlEncodeRegex, function(match, val){
						return htmlEncodeMap[val];
					});
				}else{
					return str;
				}
			},
	
			/**
			* 把字符串中的html字符解码
			* @param   str  {String}
			* @return  返回解码后的字符串
			*/
			htmlDecode: function(str) {
				if(Object.prototype.toString.call(str) === '[object String]'){
					return str.replace(htmlDecodeRegex, function(match, val){
						return htmlDecodeMap[val];
					});
				}else{
					return str;
				}
			}
		};
	})();
	
	/**
	* 日期格式化和解析，提供format和parse进行日期转换。
	* format(date, pattern)把日期格式化成字符串。
	* 使用方法：
	* var date = new Date();
	* Rhui.Date.format(date, 'yyyy-MM-dd HH:mm:ss'); //2015-08-12 13:00:00
	*
	* parse(str, pattern)把字符串转成日期。
	* 使用方法：
	* var str = 2015-08-12 13:00:00;
	* Rhui.Date.format(str, 'yyyy-MM-dd HH:mm:ss');
	* 
	* parse有两个参数，如果只传递str参数，会调用浏览器内置的Date.parse()方法进行转换。
	*
	*   格式       描述
	*   --------   ---------------------------------------------------------------
	*   yy         年份后两位，如2015取后两位是15。
	*   yyyy       年份四位。
	*   M          月份，取值1 ~ 12。
	*   MM         月份，取值01 ~ 12，如果月份为个位数，前面补0。
	*   MMM        月份缩写，如一月的英文缩写为Jan，中文缩写为一。
	*   MMMM       月份全称，如January、一月。
	*   d          日期在月中的第几天，取值1~31。
	*   dd         日期在月中的第几天，取值01~31，如果天数为个位数，前面补0。
	*   ddd        星期缩写，取值日、一、二、三、四、五、六。
	*   dddd       星期全称，取值星期日、星期一、星期二、星期三、星期四、星期五、星期六。
	*   H          24小时进制，取值0~23。
	*   HH         24小时进制，取值00~23，如果小时为个位数，前面补0。
	*   h          12小时进制，取值0~11。
	*   hh         12小时进制，取值00~11，如果小时为个位数，前面补0。
	*   m          分钟，取值0~59。
	*   mm         分钟，取值00~59，如果为个位数，前面补0。
	*   s          秒，取值0~59。
	*   ss         秒，取值00~59，如果为个位数，前面补0。
	*   S          毫秒，取值0~999。
	*   SS         毫秒，取值00~999，如果不足两位数，前面补0。
	*   SSS        毫秒，取值000~999，如果不足三位数，前面补0。
	*   t          上午、下午缩写。
	*   tt         上午、下午全称。
	*   --------   ---------------------------------------------------------------
	*
	* @author accountwcx@qq.com
	* @date   2015-08-12
	*/
	Rhui.Date = (function(){
		/*
		var locale = {
			dayNames: ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"],
			shortDayNames: ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"],
			monthNames: ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"],
			shortMonthNames: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"],
			am: 'AM',
			pm: 'PM',
			shortAm: 'A',
			shortPm: 'P'
		};
		*/
		
		var locale = {
			dayNames: ["星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六"],
			shortDayNames: ["日", "一", "二", "三", "四", "五", "六"],
			monthNames: ["一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月"],
			shortMonthNames: ["一", "二", "三", "四", "五", "六", "七", "八", "九", "十", "十一", "十二"],
			am: "上午",
			pm: "下午",
			shortAm: '上',
			shortPm: '下'
		};
		
		/**
		* 左边补0
		*/
		function leftPad(str, size){
			var result = '' + str;
			
			while (result.length < size) {
				result = '0' + result;
			}
			
			return result;
		}
		
		var parseToken = (function(){
			var match2 = /\d{2}/,          // 00 - 99
				//match3 = /\d{3}/,          // 000 - 999
				match4 = /\d{4}/,          // 0000 - 9999
				match1to2 = /\d{1,2}/,     // 0 - 99
				match1to3 = /\d{1,3}/,     // 0 - 999
				//match1to4 = /\d{1,4}/,     // 0 - 9999
				match2w = /.{2}/,         // 匹配两个字符
				match1wto2w = /.{1,2}/,   // 匹配1~2个字符
				map = {
					//年的后两位
					'yy': {
						regex: match2,
						name: 'year'
					},
					//年
					'yyyy': {
						regex: match4,
						name: 'year'
					},
					//两位数的月，不到两位数则补0
					'MM': {
						regex: match2,
						name: 'month'
					},
					//月
					'M': {
						regex: match1to2,
						name: 'month'
					},
					//两位数的日期，不到两位数则补0
					'dd': {
						regex: match2,
						name: 'date'
					},
					//日期
					'd': {
						regex: match1to2,
						name: 'date'
					},
					//两位数的小时，24小时进制
					'HH': {
						regex: match2,
						name: 'hours'
					},
					//小时，24小时进制
					'H': {
						regex: match1to2,
						name: 'hours'
					},
					//两位数的小时，12小时进制
					'hh': {
						regex: match2,
						name: 'hours'
					},
					//小时，12小时进制
					'h': {
						regex: match1to2,
						name: 'hours'
					},
					//两位数的分钟
					'mm': {
						regex: match2,
						name: 'minutes'
					},
					//分钟
					'm': {
						regex: match1to2,
						name: 'minutes'
					},
					's': {
						regex: match1to2,
						name: 'seconds'
					},
					'ss': {
						regex: match2,
						name: 'seconds'
					},
					//上午、下午
					'tt': {
						regex: match2w,
						name: 't'
					},
					//上午、下午
					't': {
						regex: match1wto2w,
						name: 't'
					},
					//毫秒
					'S': {
						regex: match1to3,
						name: 'millisecond'
					},
					//毫秒
					'SS': {
						regex: match1to3,
						name: 'millisecond'
					},
					//毫秒
					'SSS': {
						regex: match1to3,
						name: 'millisecond'
					}
				};
			
			return function(token, str, dateObj){
				var result, part = map[token];
				if(part){
					result = str.match(part.regex);
					if(result){
						dateObj[part.name] = result[0];
						return result[0];
					}
				}
				
				return null;
			};
		})();
		
		return {
			locale: locale,
			format: function(val, pattern){
				if(Object.prototype.toString.call(val) !== '[object Date]'){
					return '';
				}
				
				if(Object.prototype.toString.call(pattern) !== '[object String]' || pattern === ''){
					pattern = 'yyyy-MM-dd HH:mm:ss';
				}
				
				var fullYear = val.getFullYear(),
					month = val.getMonth(),
					day = val.getDay(),
					date = val.getDate(),
					hours = val.getHours(),
					minutes = val.getMinutes(),
					seconds = val.getSeconds(),
					milliseconds = val.getMilliseconds();
				
				return pattern.replace(/(\\)?(dd?d?d?|MM?M?M?|yy?y?y?|hh?|HH?|mm?|ss?|tt?|SS?S?)/g, function (m) {
					if (m.charAt(0) === '\\') {
						return m.replace('\\', '');
					}
					
					var locale = Rhui.Date.locale;
					
					switch (m) {
						case "hh":
							return leftPad(hours < 13 ? (hours === 0 ? 12 : hours) : (hours - 12), 2);
						case "h":
							return hours < 13 ? (hours === 0 ? 12 : hours) : (hours - 12);
						case "HH":
							return leftPad(hours, 2);
						case "H":
							return hours;
						case "mm":
							return leftPad(minutes, 2);
						case "m":
							return minutes;
						case "ss":
							return leftPad(seconds, 2);
						case "s":
							return seconds;
						case "yyyy":
							return fullYear;
						case "yy":
							return (fullYear + '').substring(2);
						case "dddd":
							return locale.dayNames[day];
						case "ddd":
							return locale.shortDayNames[day];
						case "dd":
							return leftPad(date, 2);
						case "d":
							return date;
						case "MMMM":
							return locale.monthNames[month];
						case "MMM":
							return locale.shortMonthNames[month];
						case "MM":
							return leftPad(month + 1, 2);
						case "M":
							return month + 1;
						case "t":
							return hours < 12 ? locale.shortAm : locale.shortPm;
						case "tt":
							return hours < 12 ? locale.am : locale.pm;
						case "S":
							return milliseconds;
						case "SS":
							return leftPad(milliseconds, 2);
						case "SSS":
							return leftPad(milliseconds, 3);
						default: 
							return m;
					}
				});
			},
			
			parse: function(val, pattern){
				if(!val){
					return null;
				}
				
				if(Object.prototype.toString.call(val) === '[object Date]'){
					// 如果val是日期，则返回。
					return val;
				}
				
				if(Object.prototype.toString.call(val) !== '[object String]'){
					// 如果val不是字符串，则退出。
					return null;
				}
				
				var time;
				if(Object.prototype.toString.call(pattern) !== '[object String]' || pattern === ''){
					// 如果fmt不是字符串或者是空字符串。
					// 使用浏览器内置的日期解析
					time = Date.parse(val);
					if(isNaN(time)){
						return null;
					}
					
					return new Date(time);
				}
				
				var i, token, tmpVal, 
					tokens = pattern.match(/(\\)?(dd?|MM?|yy?y?y?|hh?|HH?|mm?|ss?|tt?|SS?S?|.)/g),
					dateObj = {
						year: 0,
						month: 1,
						date: 0,
						hours: 0,
						minutes: 0,
						seconds: 0,
						millisecond: 0
					};
					
				for(i = 0; i < tokens.length; i++){
					token = tokens[i];
					tmpVal = parseToken(token, val, dateObj);
					
					if(tmpVal !== null){
						if(val.length > tmpVal.length){
							val = val.substring(tmpVal.length);
						}else{
							val = '';
						}
					}else{
						val = val.substring(token.length);
					}
				}
				
				if(dateObj.t){
					if(Rhui.Date.locale.pm === dateObj.t || Rhui.Date.locale.shortPm === dateObj.t){
						dateObj.hours = (+dateObj.hours) + 12;
					}
				}
				
				dateObj.month -= 1;
				
				return new Date(dateObj.year, dateObj.month, dateObj.date, dateObj.hours, dateObj.minutes, dateObj.seconds, dateObj.millisecond);
			}
		};
	})();
})(window, $);

(function (window, $, Rhui) {
	'use strict';
	
	/**
	 * Rhui所有组件都声明在Rhui.component中
	 */
	Rhui.component = {};
	
	/**
	 * 组件注册管理
	 * registerManager中保存每个声明的组件类引用
	 * 如Loading组件保存为registerManager['loading'] = Rhui.component.Loading
	 * loading为Loading组件的名称，每个Rhui组件都有一个唯一名称
	 * 组件通过Rhui.register注册
	 * 组件通过Rhui.unregister注销
	 * 已经注册的组件可通过Rhui.create方法创建
	 */
	Rhui.registerManager = {};
	
	/**
	 * Rhui组件前缀名称
	 * 该前缀用在jQuery对象的data里保存组件实例
	 * 如 $('#id').data(前缀名称 + 组件名称, 组件实例)
	 */
	Rhui.prefixName = 'rhui.';
	
	/**
	 * @public
	 * 注册组件
	 * 如果组件名称已存在，则覆盖已存在的组件
	 * @param name       {String}  组件名称
	 * @param component  {Class}   组件类
	 */
	Rhui.register = function(name, component){
		if(!Rhui.isString(name) || !Rhui.isFunction(component)){
			return;
		}
		
		Rhui.registerManager[name] = component;
	};
	
	/**
	 * @public
	 * 注销组件
	 * 从组件管理中删除已经注册的组件
	 * @param name {String}  组件名称
	 */
	Rhui.unregister = function(name){
		if(!name.isString(name)){
			return;
		}
		
		if(Rhui.registerManager[name] !== undefined){
			Rhui.registerManager[name] = undefined;
			delete Rhui.registerManager[name];
		}
	};
	
	/**
	 * Rhui组件实例管理
	 * 每个创建的Rhui组件都会注册到实例管理中，如果实例被销毁了，实例管理中的实例也会被移除
	 * 实例可以通过Rhui.getById()获取
	 */
	Rhui.instanceManager = {};
	
	/**
	 * @public
	 * 通过id获取Rhui组件实例，id对应的组件实例不存在则返回undefined
	 * @return {Component}
	 */
	Rhui.getById = function(id){
		return Rhui.instanceManager[id];
	};
	
	/**
	 * @public
	 * 获取Rhui组件实例
	 * @param   name     {String}  组件名称。
	 * @param   target   {String|jQuery|HTMLElement}
	 *                   目标元素。可以是HTML元素、jQuery实例、HTML元素id或者jQuery选择器。
	 * @return  如果存在组件则返回组件实例，否则返回null。
	 */
	Rhui.getCmp = function(name, target){
		if(!Rhui.isString(name)){
			return null;
		}
		
		var Cmp = Rhui.registerManager[name];
		if(!Cmp){
			return null;
		}
		
		if(!Rhui.isFunction(Cmp.initTarget)){
			return null;
		}
		
		var $target = Cmp.initTarget(target);
		if(!$target){
			return null;
		}
		
		return $target.data(Rhui.prefixName + name);
	};
	
	/**
	 * @private
	 * 把创建的组件添加到实例管理中，如果新添加的组件id与已存在的id重复，则覆盖之前的组件
	 * @param component  {Component} Rhui组件实例
	 */
	Rhui.addInstance = function(component){
		Rhui.instanceManager[component.id] = component;
	};
	
	/**
	 * @private
	 * 从实例管理中删除组件
	 * @param  component  {Component|String}  Rhui组件实例或者组件id
	 */
	Rhui.removeInstance = function(component){
		var tmp, id;
		if(Rhui.isString(component)){
			id = component;			
		}else if(component !== null && component !== undefined && Rhui.isString(component.id)){
			id = component.id;
		}
		
		if(id){
			tmp = Rhui.instanceManager[id];
			if(tmp !== undefined){
				Rhui.instanceManager[id] = undefined;
				delete Rhui.instanceManager[id];
			}
		}
	};
	
	/**
	 * @public
	 * 创建组件
	 * 所创建的组件必须在Rhui.registerManager中注册，否则返回null
	 * 如果组件名称不存在，则返回null
	 * @param   name    {String}  组件名称
	 * @param   target  {String|jQuery|HTMLElement}
	 *                  组件的目标元素，可以是HTML元素、jQuery实例、HTML元素id或者jQuery选择器
	 *                  如果匹配多个对象，只作用于第一个
	 * @param   opts    {Object}  组件初始化参数。如果该参数为空，则用组件默认初始化参数
	 * @return  组件实例或者null
	 */
	Rhui.create = function(name, target, opts){
		if(!Rhui.isString(name)){
			return null;
		}
		
		var Cmp = Rhui.registerManager[name];
		if(!Cmp){
			return null;
		}
		
		return new Cmp(target, opts);
	};
	
	$.fn.extend({
		/**
		 * @public
		 * 从jQuery实例创建组件
		 * 如果jQuery查询到多个元素，Rhui组件只会创建在jQuery查询的第一个元素
		 * 该方法最后调用Rhui.create(name, target, opts)方法
		 * @param   name  {String}  组件名称
		 * @param   opts  {Object}  组件初始化参数。如果该参数为空，则用组件默认初始化参数
		 * @return  组件实例或者null
		 */
		rhui: function(name, opts){
			if(this.length === 0){
				return;
			}
			
			/*
			var $first = this.first(),
				fullName = Rhui.prefixName + name,
				component = $first.data(fullName);
			if(component){
				return component;
			}
			*/
			
			return Rhui.create(name, this, opts);
		}
	});
	
	/**
	 * @public
	 * 实现继承
	 * @param  subType    {Function}  子类构造函数
	 * @param  superType  {Function}  父类构造函数
	 */
	Rhui.inherit = function(subType, superType){
		function F(){}
		F.prototype = superType.prototype;
		
		var p = new F();
		p.constructor = subType;
		
		subType.prototype = p;
	};
})(window, $, window.Rhui);

(function(window, $, Rhui){
	'use strict';
	
	/**
	 * Rhui抽象组件
	 */
	var Component = Rhui.component.AbstractComponent = function(target, opts){
		//组件作用对象
		this.target = target;
		
		//组件初始化参数
		this.options = opts;
				
		//设置id
		this.id = opts.id;
		if(!Rhui.isString(this.id) || this.id === '' || this.id === 'undefined' || this.id === 'null'){
			this.id = 'rhui' + Rhui.uuid().replace(/-/g, '').toLowerCase();
		}
		
		//组件自身的jQuery对象
		//有些时候$self和$target指向同一个对象
		//this.$self = undefined;
		
		//组件遮盖层
		//this.$overlay = undefined;
		
		//宽度，单位像素
		//this.width = undefined;
		
		//高度，单位像素
		//this.height = undefined;
		
		//组件是否隐藏
		//this.hidden = undefined;
		
		//组件的事件
		this.events = opts.events || {};
		
		//组件目标jQuery对象
		//相当于$target = $(target);
		if(this.$target && this.$target instanceof $){
			this.$target.data(this.fullName, this);
		}
		
		//管理当前组件
		Rhui.addInstance(this);
		
		if(Rhui.isFunction(this.events.onInit)){
			this.events.onInit.call(this, this);
		}
	};
	
	Component.cmpName = 'abstract';
	Component.prototype.name = Component.cmpName;
	Component.fullName = Rhui.prefixName + 'abstract';
	Component.prototype.fullName = Component.fullName;
	Component.className = 'Rhui.component.AbstractComponent';
	Component.prototype.className = Component.className;
	
	Component.prototype.defaults = {
		id: undefined,
		cls: undefined,
		style: undefined,
		events: {
			onInit: undefined,
			onShow: undefined,
			onHide: undefined,
			onDestroy: undefined,
			onResize: undefined,
			onExpand: undefined,
			onCollapse: undefined
		}
	};
	
	/**
	 * @public
	 * 获取id
	 * @return  {String}
	 */
	Component.prototype.getId = function(){
		return this.id;
	};
	
	/**
	 * @public
	 * 获取宽度，单位像素
	 * @return  {Number}
	 */
	Component.prototype.getWidth = function(){
		return this.width;
	};
	
	/**
	 * @public
	 * 设置宽度，单位像素
	 * @param   width  {Number}  宽度，单位像素
	 * @return  当前组件
	 */
	Component.prototype.setWidth = function(width){
		if(Rhui.isNumber(width) && width > 0){
			this.width = width;
			this.$self.outerWidth(width, true);
		}
		return this;
	};
	
	/**
	 * @public
	 * 获取高度，单位像素
	 * @return  {Number}
	 */
	Component.prototype.getHeight = function(){
		return this.height;
	};
	
	/**
	 * @public
	 * 设置高度，单位像素
	 * @param   height  {Number}  高度，单位像素
	 * @return  当前组件
	 */
	Component.prototype.setHeight = function(height){
		if(Rhui.isNumber(height) && height > 0){
			this.height = height;
			this.$self.outerHeight(height, true);
		}
		return this;
	};
	
	/**
	 * @public
	 * 把组件移动到指定元素下
	 * @param   {String|jQuery|HTMLElement}  目标元素
	 * @return  返回当前组件
	 */
	Component.prototype.renderTo = function(element){
		var $el;
		
		if(element === undefined || element === null){
			return this;
		}
		
		if(Rhui.isString(element) && $.trim(element) !== ''){
			$el = $(element);
		}else if(element.jquery || element instanceof $){
			$el = element;
		}else if(Rhui.isHTMLElement(element)){
			$el = $(element);
		}
		
		if($el === undefined || $el.length === 0){
			return this;
		}
		
		if($el.length > 1){
			$el = $el.first();
		}
		
		$el.append(this.$self);
		return this;
	};
	
	/**
	 * @public
	 * 重置组件的大小
	 * 支持链式调用
	 * @param   width         {Number}   [必填]宽度，单位像素
	 * @param   height        {Number}   [必填]高度，单位像素
	 * @param   preventEvent  {Boolean}  [可选]true表示阻止onResize事件触发，留空或者false表示允许触发onResize事件
	 * @return  返回当前组件
	 */
	Component.prototype.resize = function(width, height, preventEvent){
		var self = this, $self = self.$self;
		if(Rhui.isNumber(width) && width > 0){
			self.width = width;
			$self.outerWidth(width, true);
		}
		
		if(Rhui.isNumber(height) && height > 1){
			self.height = height;
			$self.outerHeight(height, true);
		}
		
		if(!preventEvent && Rhui.isFunction(self.events.onResize)){
			self.events.onResize.call(self, self);
		}
		
		return this;
	};
	
	/**
	 * @public
	 * 把组件的高度和宽度设置为auto，触发onResize事件
	 * 支持链式调用
	 * @param   preventEvent  {Boolean}  [可选]true表示阻止onResize事件触发，留空或者false表示允许触发onResize事件
	 * @return  返回当前组件
	 */
	Component.prototype.autosize = function(preventEvent){
		var self = this, $self = self.$self;
		$self.width('auto');
		$self.height('auto');
		
		self.width = $self.outerWidth(true);
		self.height = $self.outerHeight(true);
		
		if(!preventEvent && Rhui.isFunction(self.events.onResize)){
			self.events.onResize.call(self, self);
		}
		
		return self;
	};
	
	/**
	 * @public
	 * 显示组件，并触发onShow事件
	 * 支持链式调用
	 * @param   preventEvent  {Boolean}  [可选]true表示阻止onShow事件触发，留空或者false表示允许触发onShow事件
	 * @return  返回当前组件
	 */
	Component.prototype.show = function(preventEvent){
		this.$self.show();
		if(this.$overlay){
			this.$overlay.show();
		}
		this.hidden = false;
		if(preventEvent !== true && Rhui.isFunction(this.events.onShow)){
			this.events.onShow.call(this, this);
		}
		
		return this;
	};
	
	/**
	 * @public
	 * 隐藏组件，并触发onHide事件
	 * 支持链式调用
	 * @param   preventEvent  {Boolean}  [可选]true表示阻止onHide事件触发，留空或者false表示允许触发onHide事件
	 * @return  返回当前组件
	 */
	Component.prototype.hide = function(preventEvent){
		this.$self.hide();
		if(this.$overlay){
			this.$overlay.hide();
		}
		this.hidden = true;
		if(preventEvent !== true && Rhui.isFunction(this.events.onHide)){
			this.events.onHide.call(this, this);
		}
		
		return this;
	};
	
	/**
	 * @public
	 * 判断组件是否隐藏
	 * @return 隐藏返回true，否则返回false
	 */
	Component.prototype.isHidden = function(){
		return this.hidden;
	};
	
	/**
	 * @public
	 * 添加事件，如果该事件已存在，则替换已有事件
	 * 支持链式调用
	 * @param   name  {String}    事件名称
	 * @param   fn    {Function}  事件调用函数
	 * @return  返回当前组件
	 */
	Component.prototype.addEvent = function(name, fn){
		if(Rhui.isString(name) && Rhui.isFunction(fn)){
			this.events[name] = fn;
		}
		
		return this;
	};
	
	/**
	 * @public
	 * 添加多个事件，如果事件已存在，则替换已有事件
	 * 支持链式调用
	 * @param   opts  {String}   事件配置，如{onHide: fn, onShow: fn}
	 * @return  返回当前组件
	 */
	Component.prototype.addEvents = function(opts){
		if(Rhui.isObject(opts)){
			$.extend(this.events, opts);
		}
		return this;
	};
	
	/**
	 * @public
	 * 移除事件
	 * 支持链式调用
	 * @param   name  {String}   事件名称
	 * @return  返回当前组件
	 */
	Component.prototype.removeEvent = function(name){
		if(Rhui.isString(name)){
			this.events[name] = undefined;
		}
		return this;
	};
	
	/**
	 * @public
	 * 触发事件
	 * 支持链式调用
	 * @param   name  {String}   事件名称
	 * @return  返回当前组件
	 */
	Component.prototype.triggerEvent = function(name){
		var event;
		if(Rhui.isString(name)){
			event = this.events[name];
			if(Rhui.isFunction(event)){
				event.call(this, this);
			}
		}
		return this;
	};
	
	/**
	 * @protected
	 * 销毁组件，并触发onDestroy事件
	 * 该方法只删除AbstractComponent中定义的内容
	 * 继承类可以复用该方法
	 * @param   preventEvent  {Boolean}  [可选]true表示阻止onDestroy事件触发，留空或者false表示允许触发onDestroy事件
	 */
	Component.prototype.baseDestroy = function(preventEvent){
		this.target = undefined;
		this.options = undefined;
		
		if(this.$overlay){
			this.$overlay.remove();
			this.$overlay = undefined;
		}
		
		this.$target.removeData(this.fullName);
		this.$target = undefined;
		
		this.$self.remove();
		this.$self = undefined;
		
		Rhui.removeInstance(this.id);
		
		var onDestroy = this.events.onDestroy;
		this.events = undefined;		
		if(preventEvent !== true && Rhui.isFunction(onDestroy)){
			onDestroy.call(this, this);
		}
	};
})(window, $, window.Rhui);

