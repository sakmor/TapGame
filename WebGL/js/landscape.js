/* 横竖提示处理
	* isVertical：不传默认为false，即显示竖屏提示，（false: 提示用户横屏， true：提示用户竖屏）
	*/
(function landscape(config) {
	if ((navigator.userAgent.match(/(phone|pad|pod|iPhone|iPod|ios|iPad|Android|Mobile|BlackBerry|IEMobile|MQQBrowser|JUC|Fennec|wOSBrowser|BrowserNG|WebOS|Symbian|Windows Phone)/i))) {
		var isVertical = Number(document.getElementById("customerLandscape").getAttribute('isVertical')) || 0;
		var showWay = isVertical ? "@media screen and (min-aspect-ratio: 12/7){#orientLayer{display:block;} }" :
			"@media all and (orientation : portrait){#orientLayer{display: block;} }";
		var color = config && config.color ? config.color : "#000",
			txt = isVertical ? "為了更好的體驗，請使用垂直瀏覽" : "為了更好的遊戲體驗，僅支援水平瀏覽",
			images = config && config.images ? config.images :
				"data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAIYAAADaCAMAAABU68ovAAAAXVBMVEUAAAD29vb////x8fH////////x8fH5+fn29vby8vL////5+fn39/f6+vr////x8fH////////+/v7////09PT////x8fH39/f////////////////////x8fH///+WLTLGAAAAHXRSTlMAIpML+gb4ZhHWn1c2gvHBvq1uKJcC6k8b187lQ9yhhboAAAQYSURBVHja7d3blpowFIDhTUIAOchZDkre/zE7ycySrbUUpsRN2/1fzO18KzEqxEVgTiZNfgmmtxRc8iaR8HNe8x4BtjQePKayYCIoyBSgvNNE1AkNSHqZyLqk97EgUCCHBzZ5mkg7ScvIJuIyOyXBRFxgpqWZyGsAZLB1KjsJi8nutHU4JCRbFRH8tmirI9k8Jx2sqNs8K/m0LQkrktO2crgcgXGB4AiTEsB0hJfo9MGgX7CGcYiYwQxmMOOvZwRhBG8tCoMXjBDeXvWCEcHbi14wgCBmMIMZzGAGM5jxETNwzMAxA8cMHDNwzMAxA8cMHDNwzMAxA8cMHDNwzMAxY6E2rUQxnH2tz9cirlJFwFBJedaPnUv0M7++egPDE8iAJcIDmxwH5wwv9vUviw2kLbVO3TJU5uul/EyB0FoLp4x60PdGUd3qPurrWyjGGTc05u+1dcgI7/+tCCPARWGhH7o5Y7RCf+bH9ctXLp6v2BVDxfqz0oPXeSVaNtINo/1SXDv4dck8IIkbhtC2ol+iouEonTBCbYvVMnXOjxww6s/RFrBUpXHh/gw1rHj5d/qhYn9Gpk2FWh6xRBRX5Oj3Znh2Sq49/L6+y8pB26q9GbE2dbA2mVbx6I+7MfBglLCttm73ZQi7AD3iL4HqjFYJHSPRppqaUaJ3ATpGa+ckpGak2hRRMyqjGMkvl+xyFeSMwjAqcsZgGDdyhl0oNTnDN4yenJGZFGxNChP5/Y3efh6SM2rDOJMzboYxkDMqwyjIGcIw6F+io2FU1IxIm1JqRmgXSkvNKNCXeTpGrU0JNSO2c6LIGPgCS8AuDHz9ta0SXWDtxoDRH+MqlbC2Dt2G2JFRadtQZt2qq/orGowdGb2euxYiqWEpVWhTBnszoNAPdStuQwxqf0aocdWKW4Z+DfszIh8pxJqbuCE4YAC+4bm0evtipjpgJHeFnyyt1Ku2xa0bhjxr27p75rECNwyI9ZwvXkHq+7aTaMEV44YYy/spfgjgjNHaWW+GeUhGEX7tLlVinIFDDSgnOwhi1V6bU0b6tVS9eAERe863g4dRrtiHdc6o+nn5vtyVVgR79Cqt4uL6gfHPQyGqtP2vf7HADGbcYwaOGThm4JiBYwaOGThm4JiBYwaOGThm4JiBYwaOGThm4JiBYwaOGThm4JjhtOM+J/AgT008yDMkN/dPP9hzS8zAMQN3OEYeekp5YU7KOKXwVXqiY+QS7smcinGKABWdiBgpPJTSMHJ4KidhhPBUSMLw4CmPhKHgKUXCkHsygum71ftNSgCX6bsl8FQyfbcL5EdYsDk0R3j7aiA5wpt5AjKg/2gLJEBD/0Hf2OOf/vRrj6z/7GtP4B3nMKyjHA12kIPSjnJs3FEO0TvKkYJHOWCR+rjJH0Vn6fI5PjNbAAAAAElFTkSuQmCC";
		// style
		var nodeStyle = document.createElement('style');
		nodeStyle.setAttribute('type', 'text/css');
		nodeStyle.innerHTML =
			'@-webkit-keyframes rotation{10%{transform: rotate(90deg); -webkit-transform: rotate(90deg)} 50%, 60%{transform: rotate(0deg); -webkit-transform: rotate(0deg)} 90%{transform: rotate(90deg); -webkit-transform: rotate(90deg)} 100%{transform: rotate(90deg); -webkit-transform: rotate(90deg)} } @keyframes rotation{10%{transform: rotate(90deg); -webkit-transform: rotate(90deg)} 50%, 60%{transform: rotate(0deg); -webkit-transform: rotate(0deg)} 90%{transform: rotate(90deg); -webkit-transform: rotate(90deg)} 100%{transform: rotate(90deg); -webkit-transform: rotate(90deg)} } #orientLayer{display: none; z-index: 999999;} ' +
			showWay +
			' .mod-orient-layer{display: none; position: fixed; height: 100%; width: 100%; left: 0; top: 0; right: 0; bottom: 0; background: ' +
			color +
			'; z-index: 9997} .mod-orient-layer__content{position: absolute; width: 100%; top: 45%; margin-top: -75px; text-align: center} .mod-orient-layer__icon-orient{background-image: url(' +
			images +
			'); display: inline-block; width: 67px; height: 109px; transform: rotate(90deg); -webkit-transform: rotate(90deg); -webkit-animation: rotation infinite 1.5s ease-in-out; animation: rotation infinite 1.5s ease-in-out; -webkit-background-size: 67px; background-size: 67px} .mod-orient-layer__desc{margin-top: 20px; font-size: 40px; color: #fff}'
		document.getElementsByTagName('body')[0].appendChild(nodeStyle);
		// dom
		var nodeDom = document.createElement('div');
		nodeDom.setAttribute('id', 'orientLayer');
		nodeDom.setAttribute('class', 'mod-orient-layer');
		nodeDom.innerHTML =
			'<div class="mod-orient-layer__content"> <i class="icon mod-orient-layer__icon-orient"></i> <div class="mod-orient-layer__desc">' +
			txt + '</div> </div>';
		document.getElementsByTagName('body')[0].appendChild(nodeDom);
	}
}())