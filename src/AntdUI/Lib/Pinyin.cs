// Copyright (C) Tom <17379620>. All Rights Reserved.
// AntdUI WinForm Library | Licensed under Apache-2.0 License
// Gitee: https://gitee.com/AntdUI/AntdUI
// GitHub: https://github.com/AntdUI/AntdUI
// GitCode: https://gitcode.com/AntdUI/AntdUI

using System;
using System.Collections.Generic;
using System.Text;

namespace AntdUI
{
    public static class Pinyin
    {
        /// <summary>
        /// 取中文文本的拼音首字母
        /// </summary>
        /// <param name="text">编码为UTF8的文本</param>
        /// <returns>返回中文对应的拼音首字母</returns>
        public static string GetInitials(string text)
        {
            text = text.Trim();
            StringBuilder chars = new StringBuilder();
            for (var i = 0; i < text.Length;)
            {
                var py = MatchWord(text, i, out var len, out var initials);
                if (py == null)
                {
                    py = GetPinyin(text[i]);
                    len = 1;
                }
                if (initials != null) chars.Append(initials);
                else if (py != null && py != "") chars.Append(py[0]);
                i += len;
            }
            return chars.ToString();
        }

        /// <summary>
        /// 取中文文本的拼音首字母
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="encoding">源文本的编码</param>
        /// <returns>返回encoding编码类型中文对应的拼音首字母</returns>
        public static string GetInitials(string text, Encoding encoding)
        {
            string temp = ConvertEncoding(text, encoding, Encoding.UTF8);
            return ConvertEncoding(GetInitials(temp), Encoding.UTF8, encoding);
        }

        /// <summary>
        /// 取中文文本的拼音
        /// </summary>
        /// <param name="text">编码为UTF8的文本</param>
        /// <returns>返回中文文本的拼音</returns>
        public static string GetPinyin(string text)
        {
            var sbPinyin = new StringBuilder();
            for (var i = 0; i < text.Length;)
            {
                var py = MatchWord(text, i, out var len, out _);
                if (py == null)
                {
                    py = GetPinyin(text[i]);
                    len = 1;
                }
                if (py != "") sbPinyin.Append(py);
                i += len;
            }
            return sbPinyin.ToString().Trim();
        }

        /// <summary>
        /// 取中文文本的拼音
        /// </summary>
        /// <param name="text">编码为UTF8的文本</param>
        /// <param name="encoding">源文本的编码</param>
        /// <returns>返回encoding编码类型的中文文本的拼音</returns>
        public static string GetPinyin(string text, Encoding encoding)
        {
            string temp = ConvertEncoding(text.Trim(), encoding, Encoding.UTF8);
            return ConvertEncoding(GetPinyin(temp), Encoding.UTF8, encoding);
        }

        /// <summary>
        /// 取和拼音相同的汉字列表
        /// </summary>
        /// <param name="pinyin">编码为UTF8的拼音</param>
        /// <returns>取拼音相同的汉字列表，如拼音“ai”将会返回“唉爱……”等</returns>
        public static string GetChineseText(string pinyin)
        {
            string key = pinyin.Trim().ToLower();
            foreach (string str in codes)
            {
                if (str.StartsWith(key + " ") || str.StartsWith(key + ":")) return str.Substring(7);
            }
            return "";
        }

        /// <summary>
        /// 取和拼音相同的汉字列表，编码同参数encoding
        /// </summary>
        /// <param name="pinyin">编码为encoding的拼音</param>
        /// <param name="encoding">编码</param>
        /// <returns>返回编码为encoding的拼音为pinyin的汉字列表，如拼音“ai”将会返回“唉爱……”等</returns>
        public static string GetChineseText(string pinyin, Encoding encoding)
        {
            string text = ConvertEncoding(pinyin, encoding, Encoding.UTF8);
            return ConvertEncoding(GetChineseText(text), Encoding.UTF8, encoding);
        }

        /// <summary>
        /// 返回单个字符的汉字拼音
        /// </summary>
        /// <param name="ch">编码为UTF8的中文字符</param>
        /// <returns>ch对应的拼音</returns>
        public static string GetPinyin(char ch)
        {
            short hash = GetHashIndex(ch);
            for (var i = 0; i < hashes[hash].Length; ++i)
            {
                short index = hashes[hash][i];
                var pos = codes[index].IndexOf(ch, 7);
                if (pos != -1) return codes[index].Substring(0, 6).Trim();
            }
            return ch.ToString();
        }

        /// <summary>
        /// 返回单个字符的汉字拼音
        /// </summary>
        /// <param name="ch">编码为encoding的中文字符</param>
        /// <param name="encoding">编码</param>
        /// <returns>编码为encoding的ch对应的拼音</returns>
        public static string GetPinyin(char ch, Encoding encoding)
        {
            ch = ConvertEncoding(ch.ToString(), encoding, Encoding.UTF8)[0];
            return ConvertEncoding(GetPinyin(ch), Encoding.UTF8, encoding);
        }

        /// <summary>
        /// 转换编码 
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="srcEncoding">源编码</param>
        /// <param name="dstEncoding">目标编码</param>
        /// <returns>目标编码文本</returns>
        public static string ConvertEncoding(string text, Encoding srcEncoding, Encoding dstEncoding)
        {
            byte[] srcBytes = srcEncoding.GetBytes(text), dstBytes = Encoding.Convert(srcEncoding, dstEncoding, srcBytes);
            return dstEncoding.GetString(dstBytes);
        }

        /// <summary>
        /// 取文本索引值
        /// </summary>
        /// <param name="ch">字符</param>
        /// <returns>文本索引值</returns>
        static short GetHashIndex(char ch) => (short)((uint)ch % codes.Length);

        #region 多音字词组

        static readonly char[] separators = new char[] { ' ', '\t', '\r', '\n' };

        static Dictionary<string, string>? polyphonicDict;
        static Dictionary<string, string>? polyphonicInitials;
        static HashSet<char>? polyphonicFirst;
        static int polyphonicMaxLen = 2;

        /// <summary>
        /// 多音字词组词典（首次使用时懒加载）
        /// </summary>
        static Dictionary<string, string> Polyphonic
        {
            get
            {
                var dict = polyphonicDict;
                if (dict == null)
                {
                    dict = new Dictionary<string, string>(4096);
                    var initials = new Dictionary<string, string>(4096);
                    var first = new HashSet<char>();
                    var max = 2;
                    foreach (var item in polyphonic.Split(separators, StringSplitOptions.RemoveEmptyEntries))
                    {
                        var index = item.IndexOf(':');
                        if (index < 1) continue;
                        var split = item.IndexOf(':', index + 1);
                        if (split < 1) continue;
                        var key = item.Substring(0, index);
                        dict[key] = item.Substring(index + 1, split - index - 1);
                        initials[key] = item.Substring(split + 1);
                        first.Add(key[0]);
                        if (key.Length > max) max = key.Length;
                    }
                    polyphonicMaxLen = max;
                    polyphonicDict = dict;
                    polyphonicInitials = initials;
                    polyphonicFirst = first;
                }
                return dict;
            }
        }

        /// <summary>
        /// 按最长匹配取词组拼音
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="index">起始位置</param>
        /// <param name="len">命中词组的长度，未命中为1</param>
        /// <param name="initials">命中词组的拼音首字母，未命中为null</param>
        /// <returns>命中返回词组拼音，未命中返回null</returns>
        static string? MatchWord(string text, int index, out int len, out string? initials)
        {
            len = 1;
            initials = null;
            var dict = Polyphonic;
            var first = polyphonicFirst;
            if (first == null || !first.Contains(text[index])) return null;
            var max = text.Length - index;
            if (max > polyphonicMaxLen) max = polyphonicMaxLen;
            for (var i = max; i > 1; i--)
            {
                var key = text.Substring(index, i);
                if (dict.TryGetValue(key, out var py))
                {
                    len = i;
                    if (polyphonicInitials != null) polyphonicInitials.TryGetValue(key, out initials);
                    return py;
                }
            }
            return null;
        }

        /// <summary>
        /// 多音字词组拼音表，格式"词组:拼音:首字母"，以空格或换行分隔
        /// </summary>
        internal static string polyphonic = @"
一个劲地:yigejinde:ygjd 一了百了:yiliaobailiao:ylbl 一传:yizhuan:yz 一佛出世:yifochushi:yfcs 一刹那:yichana:ycn 一帧:yizhen:yz 一幢:yizhuang:yz 一推了之:yituiliaozhi:ytlz
一无所长:yiwusuozhang:ywsz 一朝:yizhao:yz 一朝一夕:yizhaoyixi:yzyx 一模一样:yimuyiyang:ymyy 一沓:yida:yd 一目了然:yimuliaoran:ymlr 一目十行:yimushihang:ymsh 一笑了之:yixiaoliaozhi:yxlz
一般地说:yibandeshuo:ybds 一行:yihang:yh 一行行:yihanghang:yhh 一语中的:yiyuzhongdi:yyzd 一语破的:yiyupodi:yypd 一走了之:yizouliaozhi:yzlz 一重:yichong:yc 一针见血:yizhenjianxie:yzjx
一长制:yizhangzhi:yzz 七十二行:qishierhang:qseh 万乘:wansheng:ws 万千重:wanqianchong:wqc 万夫长:wanfuzhang:wfz 万头攒动:wantoucuandong:wtcd 万家生佛:wanjiashengfo:wjsf 万重:wanchong:wc
三万重:sanwanchong:swc 三和银行:sanheyinhang:shyh 三藏:sanzang:sz 三行:sanhang:sh 三重:sanchong:sc 三重奏:sanchongzou:scz 三长制:sanzhangzhi:szz 上不着天:shangbuzhaotian:sbzt
上海银行:shanghaiyinhang:shyh 上调:shangtiao:st 下不了台:xiabuliaotai:xblt 下不着地:xiabuzhaodi:xbzd 下调:xiatiao:xt 不了了之:buliaoliaozhi:bllz 不得了:budeliao:bdl 不曾:buceng:bc
不甚了了:bushenliaoliao:bsll 不省人事:buxingrenshi:bxrs 不着边际:buzhuobianji:bzbj 不管部长:buguanbuzhang:bgbz 不长一智:buzhangyizhi:bzyz 丑角:choujue:cj 世界银行:shijieyinhang:sjyh 世行:shihang:sh
丘道长:qiudaozhang:qdz 东柏林:dongbolin:dbl 东阿:donge:de 东阿县:dongexian:dex 丢三落四:diusanlasi:dsls 丢卒保车:diuzubaoju:dzbj 两幢:liangzhuang:lz 两肋插刀:liangleichadao:llcd
两行:lianghang:lh 两重性:liangchongxing:lcx 中信银行:zhongxinyinhang:zxyh 中国银行:zhongguoyinhang:zgyh 中央乐团:zhongyangyuetuan:zyyt 中央银行:zhongyangyinhang:zyyh 中曾:zhongceng:zc 中牟:zhongmu:zm
中牟县:zhongmuxian:zmx 中行:zhonghang:zh 中队长:zhongduizhang:zdz 丹佛:danfo:df 丹参:danshen:ds 丹参片:danshenpian:dsp 为什么:weishenme:wsm 主角:zhujue:zj
主角奖:zhujuejiang:zjj 久别重逢:jiubiechongfeng:jbcf 义薄云天:yiboyuntian:ybyt 乌咀乡:wuzuixiang:wzx 乌思藏:wusizang:wsz 乌斯藏:wusizang:wsz 乌鞘岭:wushaoling:wsl 乍暖还寒:zhanuanhuanhan:znhh
乐亭:laoting:lt 乐亭县:laotingxian:ltx 乐句:yueju:yj 乐器:yueqi:yq 乐团:yuetuan:yt 乐坛:yuetan:yt 乐声:yuesheng:ys 乐工:yuegong:yg
乐工舞:yuegongwu:ygw 乐师:yueshi:ys 乐府:yuefu:yf 乐府诗:yuefushi:yfs 乐律:yuelu:yl 乐手:yueshou:ys 乐曲:yuequ:yq 乐曲声:yuequsheng:yqs
乐歌:yuege:yg 乐段:yueduan:yd 乐毅攻:yueyigong:yyg 乐池:yuechi:yc 乐清:yueqing:yq 乐清市:yueqingshi:yqs 乐理:yueli:yl 乐章:yuezhang:yz
乐舞:yuewu:yw 乐艺:yueyi:yy 乐谱:yuepu:yp 乐队:yuedui:yd 乐陵:laoling:ll 乐音:yueyin:yy 乘务长:chengwuzhang:cwz 乜斜:miexie:mx
九重:jiuchong:jc 九重天:jiuchongtian:jct 乡镇长:xiangzhenzhang:xzz 乡长:xiangzhang:xz 书僮:shutong:st 乱弹:luantan:lt 乱弹琴:luantanqin:ltq 乳臭未干:ruxiuweigan:rxwg
了不得:liaobude:lbd 了不起:liaobuqi:lbq 了事:liaoshi:ls 了如指掌:liaoruzhizhang:lrzz 了得:liaode:ld 了悟:liaowu:lw 了断:liaoduan:ld 了无生趣:liaowushengqu:lwsq
了此一生:liaociyisheng:lcys 了然:liaoran:lr 了然于胸:liaoranyuxiong:lryx 了结:liaojie:lj 了若指掌:liaoruozhizhang:lrzz 了解:liaojie:lj 事务部长:shiwubuzhang:swbz 事务长:shiwuzhang:swz
二传:erzhuan:ez 二十八宿:ershibaxiu:esbx 二重唱:erchongchang:ecc 二重奏:erchongzou:ecz 二重性:erchongxing:ecx 于乐调:yuyuediao:yyd 于柏林:yubolin:ybl 于龟兹:yuqiuci:yqc
云寺佛:yunsifo:ysf 五重奏:wuchongzou:wcz 井木犴:jingmuhan:jmh 交口称赞:jiaokouchengzan:jkcz 交响乐:jiaoxiangyue:jxy 交响乐团:jiaoxiangyuetuan:jxyt 交响乐队:jiaoxiangyuedui:jxyd 交响音乐:jiaoxiangyinyue:jxyy
交差:jiaochai:jc 交恶:jiaowu:jw 交给:jiaogei:jg 交行:jiaohang:jh 交还:jiaohuan:jh 交通部长:jiaotongbuzhang:jtbz 交通银行:jiaotongyinhang:jtyh 亭长:tingzhang:tz
亲家:qingjia:qj 亲家母:qingjiamu:qjm 亲率:qinshuai:qs 人参:renshen:rs 人参果:renshenguo:rsg 人头攒动:rentoucuandong:rtcd 人民银行:renminyinhang:rmyh 人称:rencheng:rc
人行:renhang:rh 什么:shenme:sm 什么样:shenmeyang:smy 什刹海:shichahai:sch 仇世华:qiushihua:qsh 仇士华:qiushihua:qsh 仇士良:qiushiliang:qsl 仇子明:qiuziming:qzm
仇岂可:qiuqike:qqk 仇得报:qiudebao:qdb 仇必报:qiubibao:qbb 仇志海:qiuzhihai:qzh 仇未复:qiuweifu:qwf 仇未报:qiuweibao:qwb 仇松年:qiusongnian:qsn 仇池:qiuchi:qc
仇深似:qiushensi:qss 仇萌芽:qiumengya:qmy 今朝:jinzhao:jz 介壳:jieqiao:jq 仓卒:cangcu:cc 付给:fugei:fg 仙乐:xianyue:xy 代理行:dailihang:dlh
代称:daicheng:dc 代部长:daibuzhang:dbz 以己度人:yijiduoren:yjdr 以牙还牙:yiyahuanya:yyhy 以眼还眼:yiyanhuanyan:yyhy 任为长老:renweizhanglao:rwzl 任柏林:renbolin:rbl 任组长:renzuzhang:rzz
伎俩:jiliang:jl 伏乾归:fugangui:fgg 众矢之的:zhongshizhidi:zszd 会稽山:kuaijishan:kjs 会稽王:kuaijiwang:kjw 会计:kuaiji:kj 会计处:kuaijichu:kjc 会计学:kuaijixue:kjx
会计室:kuaijishi:kjs 会计师:kuaijishi:kjs 会计法:kuaijifa:kjf 会计系:kuaijixi:kjx 会计证:kuaijizheng:kjz 会长:huizhang:hz 传给:chuangei:cg 传记:zhuanji:zj
传记类:zhuanjilei:zjl 伸缩:shensuo:ss 伸缩式:shensuoshi:sss 伸缩性:shensuoxing:ssx 伺候:cihou:ch 似地:shide:sd 似曾相识:sicengxiangshi:scxs 似的:shide:sd
伽蓝:qielan:ql 伽马:gama:gm 伽马刀:gamadao:gmd 住友银行:zhuyouyinhang:zyyh 何曾:heceng:hc 佛事:foshi:fs 佛像:foxiang:fx 佛光:foguang:fg
佛光寺:foguangsi:fgs 佛兰德:folande:fld 佛典:fodian:fd 佛号:fohao:fh 佛国:foguo:fg 佛堂:fotang:ft 佛塔:fota:ft 佛头:fotou:ft
佛学:foxue:fx 佛学院:foxueyuan:fxy 佛家:fojia:fj 佛寺:fosi:fs 佛尔:foer:fe 佛山:foshan:fs 佛山人:foshanren:fsr 佛山市:foshanshi:fss
佛得角:fodejiao:fdj 佛手:foshou:fs 佛拉:fola:fl 佛教:fojiao:fj 佛教协会:fojiaoxiehui:fjxh 佛教史:fojiaoshi:fjs 佛教徒:fojiaotu:fjt 佛教界:fojiaojie:fjj
佛朗哥:folangge:flg 佛殿:fodian:fd 佛法:fofa:ff 佛爷:foye:fy 佛牙:foya:fy 佛珠:fozhu:fz 佛祖:fozu:fz 佛经:fojing:fj
佛罗伦萨:foluolunsa:flls 佛罗里达:foluolida:flld 佛蒙特州:fomengtezhou:fmtz 佛诞节:fodanjie:fdj 佛门:fomen:fm 佛门弟子:fomendizi:fmdz 佛陀:fotuo:ft 佛香阁:foxiangge:fxg
佛龛:fokan:fk 佯称:yangcheng:yc 佳酿:jianiang:jn 便宜:pianyi:py 便宜货:pianyihuo:pyh 便溺:bianniao:bn 便血:bianxie:bx 俗称:sucheng:sc
保长:baozhang:bz 信佛:xinfo:xf 信差:xinchai:xc 信称义:xinchengyi:xcy 俯首称臣:fushouchengchen:fscc 倒打一耙:daodayipa:ddyp 倔强:juejiang:jj 借尸还魂:jieshihuanhun:jshh
借给:jiegei:jg 借花献佛:jiehuaxianfo:jhxf 倾轧:qingya:qy 做不了:zuobuliao:zbl 偿还:changhuan:ch 偿还期:changhuanqi:chq 像模像样:xiangmuxiangyang:xmxy 僧伽罗语:sengqieluoyu:sqly
僮仆:tongpu:tp 兄长:xiongzhang:xz 充塞:chongse:cs 光大银行:guangdayinhang:gdyh 光栅:guangshan:gs 光栅扫描:guangshansaomiao:gssm 免不了:mianbuliao:mbl 兔起鹘落:tuqihuluo:tqhl
党参:dangshen:ds 全传:quanzhuan:qz 全军覆没:quanjunfumo:qjfm 全称:quancheng:qc 全行:quanhang:qh 全都:quandou:qd 八行:bahang:bh 公仔:gongzai:gz
公冶乾:gongyegan:gyg 公安局长:gonganjuzhang:gajz 公差:gongchai:gc 六安:luan:la 六安市:luanshi:las 六畜:liuchu:lc 关卡:guanqia:gq 关学曾:guanxueceng:gxc
兴业银行:xingyeyinhang:xyyh 具体地说:jutideshuo:jtds 养畜:yangchu:yc 内出血:neichuxie:ncx 内务部长:neiwubuzhang:nwbz 内政部长:neizhengbuzhang:nzbz 内省:neixing:nx 内行:neihang:nh
内行人:neihangren:nhr 写给:xiegei:xg 军乐:junyue:jy 军乐团:junyuetuan:jyt 军乐队:junyuedui:jyd 军事部长:junshibuzhang:jsbz 军团长:juntuanzhang:jtz 军长:junzhang:jz
农业部长:nongyebuzhang:nybz 农业银行:nongyeyinhang:nyyh 农发行:nongfahang:nfh 农畜:nongchu:nc 农行:nonghang:nh 冯院长:fengyuanzhang:fyz 冷冷地:lenglengde:lld 冷轧:lengya:ly
冷颤:lengzhan:lz 减缩:jiansuo:js 几幢:jizhuang:jz 几行:jihang:jh 几重:jichong:jc 出公差:chugongchai:cgc 出头露面:chutouloumian:ctlm 出差:chuchai:cc
出差费:chuchaifei:ccf 出没:chumo:cm 出没无常:chumowuchang:cmwc 出血:chuxie:cx 出血性:chuxiexing:cxx 出血点:chuxiedian:cxd 出血病:chuxiebing:cxb 出血量:chuxieliang:cxl
凼仔岛:dangzaidao:dzd 分支行:fenzhihang:fzh 分给:fengei:fg 分行:fenhang:fh 分行业:fenhangye:fhy 分队长:fenduizhang:fdz 列传:liezhuan:lz 列车长:liechezhang:lcz
刘禅:liushan:ls 刘重阳:liuchongyang:lcy 刘铭传:liumingzhuan:lmz 刘长卿:liuzhangqing:lzq 刚劲:gangjing:gj 刚正不阿:gangzhengbue:gzbe 刚直不阿:gangzhibue:gzbe 删削:shanxue:sx
刨床:baochuang:bc 刨花:baohua:bh 刨花板:baohuaban:bhb 别传:biezhuan:bz 别称:biecheng:bc 刹帝利:chadili:cdl 刹时:chashi:cs 刹时间:chashijian:csj
刹那:chana:cn 刹那间:chanajian:cnj 刻薄:kebo:kb 削价:xuejia:xj 削减:xuejian:xj 削发:xuefa:xf 削壁:xuebi:xb 削平:xueping:xp
削弱:xueruo:xr 削瘦:xueshou:xs 削足适履:xuezushilu:xzsl 削铁如泥:xuetieruni:xtrn 剥削:boxue:bx 剥削者:boxuezhe:bxz 剥削阶级:boxuejieji:bxjj 剥夺:boduo:bd
剥离:boli:bl 剥落:boluo:bl 剥蚀:boshi:bs 劝降:quanxiang:qx 劝降书:quanxiangshu:qxs 功不可没:gongbukemo:gbkm 加的夫:jiadifu:jdf 动弹:dongtan:dt
助长:zhuzhang:zz 劲射:jingshe:js 劲敌:jingdi:jd 劲旅:jinglu:jl 劲风:jingfeng:jf 势单力薄:shidanlibo:sdlb 勒死:leisi:ls 勒紧:leijin:lj
包扎:baoza:bz 北京分行:beijingfenhang:bjfh 区长:quzhang:qz 十三行:shisanhang:ssh 十四行:shisihang:ssh 十四行诗:shisihangshi:sshs 十行:shihang:sh 十里堡:shilipu:slp
千佛山:qianfoshan:qfs 千佛岩:qianfoyan:qfy 千佛洞:qianfodong:qfd 千重:qianchong:qc 午觉:wujiao:wj 华夏银行:huaxiayinhang:hxyh 华联商厦:hualianshangsha:hlss 华达呢:huadani:hdn
协调:xietiao:xt 协调会:xietiaohui:xth 协调员:xietiaoyuan:xty 协调性:xietiaoxing:xtx 协调者:xietiaozhe:xtz 单于:chanyu:cy 单于庭:chanyuting:cyt 单峰驼:shanfengtuo:sft
单廷:shanting:st 单薄:danbo:db 占便宜:zhanpianyi:zpy 卡住:qiazhu:qz 卡壳:qiake:qk 卡子:qiazi:qz 卡脖子:qiabozi:qbz 卢大夫:ludaifu:ldf
卧佛:wofo:wf 卧佛寺:wofosi:wfs 卫生局长:weishengjuzhang:wsjz 卫生部长:weishengbuzhang:wsbz 卫队长:weiduizhang:wdz 卷土重来:juantuchonglai:jtcl 厂长:changzhang:cz 厅局长:tingjuzhang:tjz
厅长:tingzhang:tz 历三朝:lisanzhao:lsz 压缩:yasuo:ys 压缩性:yasuoxing:ysx 压缩机:yasuoji:ysj 压缩空气:yasuokongqi:yskq 压缩算法:yasuosuanfa:yssf 压缩饼干:yasuobinggan:ysbg
厌恶:yanwu:yw 厚朴:houpo:hp 厚此薄彼:houcibobi:hcbb 厚积薄发:houjibofa:hjbf 厦华:shahua:sh 厦大:shada:sd 县长:xianzhang:xz 参差:cenci:cc
参差不齐:cencibuqi:ccbq 参差错落:cencicuoluo:cccl 参谋长:canmouzhang:cmz 参附汤:shenfutang:sft 又称:youcheng:yc 双重:shuangchong:sc 双重人格:shuangchongrenge:scrg 双重性:shuangchongxing:scx
反弹:fantan:ft 反省:fanxing:fx 反诘:fanjie:fj 反躬自省:fangongzixing:fgzx 发人深省:farenshenxing:frsx 发卡:faqia:fq 发卡量:faqialiang:fql 发给:fagei:fg
发胖:fapang:fp 发还:fahuan:fh 受不了:shoubuliao:sbl 受得了:shoudeliao:sdl 受降:shouxiang:sx 口称:koucheng:kc 古刹:gucha:gc 古称:gucheng:gc
句读:judou:jd 另辟蹊径:lingpixijing:lpxj 叨扰:taorao:tr 只争朝夕:zhizhengzhaoxi:zzzx 可恶:kewu:kw 可的松:kedisong:kds 可调:ketiao:kt 可调式:ketiaoshi:kts
台长:taizhang:tz 史称皇:shichenghuang:sch 号称:haocheng:hc 司务长:siwuzhang:swz 司法部长:sifabuzhang:sfbz 司长:sizhang:sz 吃里扒外:chilipawai:clpw 各行各业:gehanggeye:ghgy
合称:hecheng:hc 同行:tonghang:th 同行业:tonghangye:thy 名称:mingcheng:mc 名角:mingjue:mj 名角儿:mingjueer:mje 吐蕃:tubo:tb 吐蕃王:tubowang:tbw
吐血:tuxie:tx 吐谷浑:tuyuhun:tyh 吕调阳:lutiaoyang:lty 吞没:tunmo:tm 吟哦:yine:ye 吡咯:biluo:bl 否极泰来:pijitailai:pjtl 含情脉脉:hanqingmomo:hqmm
听差:tingchai:tc 吱吱声:zhizisheng:zzs 吱声:zisheng:zs 吴一氓:wuyimeng:wym 吴中行:wuzhonghang:wzh 吴劲草:wujingcao:wjc 吴四长老:wusizhanglao:wszl 吴基传:wujizhuan:wjz
吴长老:wuzhanglao:wzl 呆呆地:daidaide:ddd 告老还乡:gaolaohuanxiang:glhx 呜呜咽咽:wuwuyeye:wwyy 呜咽:wuye:wy 呢喃:ninan:nn 呢大衣:nidayi:ndy 呢子:nizi:nz
呢帽:nimao:nm 呢绒:nirong:nr 周佛海:zhoufohai:zfh 呱呱坠地:guguzhuidi:ggzd 呵叻:kele:kl 呼呼地:huhude:hhd 呼韩邪:huhanye:hhy 咀嚼:jujue:jj
咋呼:zhahu:zh 咋咋呼呼:zhazhahuhu:zzhh 咋唬:zhahu:zh 咋舌:zeshe:zs 和修佛:hexiufo:hxf 和召公:heshaogong:hsg 和稀泥:huoxini:hxn 咔嚓:kacha:kc
咖喱:gali:gl 咯吱:gezhi:gz 咯吱咯吱:gezhigezhi:gzgz 咯咯:gege:gg 咯噔:gedeng:gd 咯血:kaxie:kx 咱们:zanmen:zm 咱俩:zanlia:zl
咱家:zanjia:zj 咱村:zancun:zc 哀乐:aiyue:ay 哀乐声:aiyuesheng:ays 哈佛:hafo:hf 哈佛大学:hafodaxue:hfdx 哨卡:shaoqia:sq 哪吒:nezha:nz
哽咽:gengye:gy 唐三藏:tangsanzang:tsz 唐学曾:tangxueceng:txc 唐长老:tangzhanglao:tzl 唱主角:changzhujue:czj 唱喏:changre:cr 商业部长:shangyebuzhang:sybz 商业银行:shangyeyinhang:syyh
商厦:shangsha:ss 商家堡:shangjiapu:sjp 商行:shanghang:sh 商贾:shanggu:sg 啜泣:chuoqi:cq 啜泣声:chuoqisheng:cqs 啜饮:chuoyin:cy 啧啧称奇:zezechengqi:zzcq
啧啧称赞:zezechengzan:zzcz 喀嚓:kacha:kc 喘吁吁:chuanxuxu:cxx 喷薄而出:penboerchu:pbec 嘁嘁喳喳:qiqichacha:qqcc 器乐:qiyue:qy 器乐曲:qiyuequ:qyq 噱头:xuetou:xt
四氢吡咯:siqingbiluo:sqbl 四行:sihang:sh 四重:sichong:sc 四重奏:sichongzou:scz 回传:huizhuan:hz 回弹:huitan:ht 回弹性:huitanxing:htx 回调:huitiao:ht
回鹘:huihu:hh 团长:tuanzhang:tz 囤积:tunji:tj 囤积居奇:tunjijuqi:tjjq 园长:yuanzhang:yz 困难重重:kunnanchongchong:kncc 固着:guzhuo:gz 国乐:guoyue:gy
国务部长:guowubuzhang:gwbz 国家银行:guojiayinhang:gjyh 国防部长:guofangbuzhang:gfbz 图像压缩:tuxiangyasuo:txys 图穷匕见:tuqiongbixian:tqbx 圈养:juanyang:jy 圜丘:yuanqiu:yq 土生土长:tushengtuzhang:tstz
圩区:weiqu:wq 地壳:diqiao:dq 地藏王:dizangwang:dzw 场长:changzhang:cz 坚称:jiancheng:jc 坦率:tanshuai:ts 埋怨:manyuan:my 埋没:maimo:mm
堡子:buzi:bz 堪称:kancheng:kc 堪称一绝:kanchengyijue:kcyj 堰塞湖:yansehu:ysh 堵塞:duse:ds 塞佛特:saifote:sft 塞擦音:secayin:scy 塞音:seyin:sy
填塞:tianse:ts 增辟:zengpi:zp 增长:zengzhang:zz 增长期:zengzhangqi:zzq 增长极:zengzhangji:zzj 增长点:zengzhangdian:zzd 增长率:zengzhanglu:zzl 增长量:zengzhangliang:zzl
增长额:zengzhange:zze 壅塞:yongse:ys 声乐:shengyue:sy 声乐系:shengyuexi:syx 声称:shengcheng:sc 壳牌:qiaopai:qp 处长:chuzhang:cz 外交部长:waijiaobuzhang:wjbz
外传:waizhuan:wz 外出血:waichuxie:wcx 外行:waihang:wh 外行人:waihangren:whr 外行话:waihanghua:whh 外邪:waiye:wy 外长:waizhang:wz 多佛:duofo:df
多佛尔:duofoer:dfe 多重:duochong:dc 大不了:dabuliao:dbl 大会计:dakuaiji:dkj 大佛:dafo:df 大佛像:dafoxiang:dfx 大佛寺:dafosi:dfs 大佛湾:dafowan:dfw
大出血:dachuxie:dcx 大厦:dasha:ds 大厦将倾:dashajiangqing:dsjq 大城:daicheng:dc 大埔:dabu:db 大处着眼:dachuzhuoyan:dczy 大夫:daifu:df 大模大样:damudayang:dmdy
大气磅礴:daqipangbo:dqpb 大腹便便:dafupianpian:dfpp 大萝卜:daluobo:dlb 大藏:dazang:dz 大藏经:dazangjing:dzj 大长老:dazhanglao:dzl 大队长:daduizhang:ddz 天然湖泊:tianranhupo:trhp
天道好还:tiandaohaohuan:tdhh 太子参:taizishen:tzs 太行:taihang:th 太行山:taihangshan:ths 太行山区:taihangshanqu:thsq 太行山麓:taihangshanlu:thsl 夫差:fuchai:fc 央行:yanghang:yh
失调:shitiao:st 头颈:toujing:tj 头颈部:toujingbu:tjb 奇偶:jiou:jo 奇数:jishu:js 奉还:fenghuan:fh 奏乐:zouyue:zy 奖给:jianggei:jg
套色:taoshai:ts 奚长老:xizhanglao:xzl 奥立佛:aolifo:alf 女红:nugong:ng 好恶:haowu:hw 好逸恶劳:haoyiwulao:hywl 如履薄冰:rulubobing:rlbb 如来佛:rulaifo:rlf
妄称:wangcheng:wc 妄自菲薄:wangzifeibo:wzfb 妇女部长:funubuzhang:fnbz 委员长:weiyuanzhang:wyz 姚贤镐:yaoxianhao:yxh 威吓:weihe:wh 婀娜:enuo:en 婀娜多姿:enuoduozi:endz
嫌恶:xianwu:xw 嫡长子:dizhangzi:dzz 嬷嬷:momo:mm 子宫颈:zigongjing:zgj 字模:zimu:zm 字里行间:zilihangjian:zlhj 孙毓筠:sunyuyun:syy 孙长老:sunzhanglao:szl
季长老:jizhanglao:jzl 学长:xuezhang:xz 孱弱:chanruo:cr 宇称:yucheng:yc 安的列斯:andiliesi:adls 宋嬷嬷:songmomo:smm 宋长老:songzhanglao:szl 宗李乾:zongligan:zlg
官差:guanchai:gc 官房长官:guanfangzhangguan:gfzg 官长:guanzhang:gz 宝刹:baocha:bc 宝坻:baodi:bd 宝坻县:baodixian:bdx 宝藏:baozang:bz 实实地:shishide:ssd
实弹射击:shitansheji:stsj 审判长:shenpanzhang:spz 审度:shenduo:sd 审时度势:shenshiduoshi:ssds 审计长:shenjizhang:sjz 宣传部长:xuanchuanbuzhang:xcbz 宣家堡:xuanjiapu:xjp 宣称:xuancheng:xc
室长:shizhang:sz 宫颈:gongjing:gj 宫颈癌:gongjingai:gja 家畜:jiachu:jc 家长:jiazhang:jz 家长会:jiazhanghui:jzh 家长制:jiazhangzhi:jzz 家长式:jiazhangshi:jzs
密钥:miyao:my 富商巨贾:fushangjugu:fsjg 富士银行:fushiyinhang:fsyh 寒颤:hanzhan:hz 对牛弹琴:duiniutanqin:dntq 封禅:fengshan:fs 尉犁:yuli:yl 尉犁县:yulixian:ylx
尉迟:yuchi:yc 尉迟乙僧:yuchiyiseng:ycys 尉迟孙:yuchisun:ycs 尉迟孙立:yuchisunli:ycsl 尉迟恭:yuchigong:ycg 尉迟氏:yuchishi:ycs 尉迟连:yuchilian:ycl 尉迟迥:yuchijiong:ycj
尊称:zuncheng:zc 尊长:zunzhang:zz 小事化了:xiaoshihualiao:xshl 小传:xiaozhuan:xz 小便宜:xiaopianyi:xpy 小组长:xiaozuzhang:xzz 小薄氏:xiaoboshi:xbs 小队长:xiaoduizhang:xdz
少不了:shaobuliao:sbl 尖削:jianxue:jx 尖沙咀:jianshazui:jsz 尖酸刻薄:jiansuankebo:jskb 尚君长:shangjunzhang:sjz 尿泡:suipao:sp 尿血:niaoxie:nx 局长:juzhang:jz
屏住:bingzhu:bz 屏弃:bingqi:bq 屏息:bingxi:bx 屏气:bingqi:bq 屏气凝神:bingqiningshen:bqns 属意:zhuyi:zy 山大王:shandaiwang:sdw 山楂:shanzha:sz
山楂片:shanzhapian:szp 山楂糕:shanzhagao:szg 山重水复:shanchongshuifu:scsf 崆峒:kongtong:kt 崆峒山:kongtongshan:kts 川藏公路:chuanzanggonglu:czgl 川藏线:chuanzangxian:czx 州长:zhouzhang:zz
工业部长:gongyebuzhang:gybz 工尺:gongche:gc 工行:gonghang:gh 工长:gongzhang:gz 左传:zuozhuan:zz 左路传:zuoluzhuan:zlz 左长史:zuozhangshi:zzs 巨贾:jugu:jg
差事:chaishi:cs 差使:chaishi:cs 差官:chaiguan:cg 差役:chaiyi:cy 差旅费:chailufei:clf 差遣:chaiqian:cq 巴尔的摩:baerdimo:bedm 巴扎:baza:bz
巷道:hangdao:hd 市长:shizhang:sz 布率兵:bushuaibing:bsb 师长:shizhang:sz 希腊人:xixiren:xxr 帕隆藏布:palongzangbu:plzb 带给:daigei:dg 帧中继:zhenzhongji:zzj
帮不了:bangbuliao:bbl 干不了:ganbuliao:gbl 干亲家:ganqingjia:gqj 干什么:ganshenme:gsm 干着急:ganzhaoji:gzj 年增长率:nianzengzhanglu:nzzl 年长:nianzhang:nz 年长者:nianzhangzhe:nzz
并称:bingcheng:bc 幼畜:youchu:yc 广东音乐:guangdongyinyue:gdyy 广佛华:guangfohua:gfh 广厦:guangsha:gs 广渠门:anqumen:aqm 广种薄收:guangzhongboshou:gzbs 库藏:kuzang:kz
店长:dianzhang:dz 度德量力:duodeliangli:ddll 庭长:tingzhang:tz 康立乾:kangligan:klg 建行:jianhang:jh 建设银行:jiansheyinhang:jsyh 开天辟地:kaitianpidi:ktpd 开小差:kaixiaochai:kxc
开辟:kaipi:kp 开都河:kaidouhe:kdh 弄堂:longtang:lt 引吭:yinhang:yh 引吭高歌:yinhanggaoge:yhgg 引着:yinzhao:yz 引颈:yinjing:yj 张一氓:zhangyimeng:zym
张耀曾:zhangyaoceng:zyc 弥勒佛:milefo:mlf 弦乐:xianyue:xy 弦乐器:xianyueqi:xyq 弹冠相庆:tanguanxiangqing:tgxq 弹力:tanli:tl 弹劾:tanhe:th 弹压:tanya:ty
弹唱:tanchang:tc 弹回:tanhui:th 弹塑性:tansuxing:tsx 弹奏:tanzou:tz 弹射:tanshe:ts 弹性:tanxing:tx 弹性体:tanxingti:txt 弹性模量:tanxingmoliang:txml
弹拨:tanbo:tb 弹拨乐器:tanboyueqi:tbyq 弹指:tanzhi:tz 弹球:tanqiu:tq 弹琴:tanqin:tq 弹着点:danzhuodian:dzd 弹簧:tanhuang:th 弹簧秤:tanhuangcheng:thc
弹簧钢:tanhuanggang:thg 弹簧门:tanhuangmen:thm 弹词:tanci:tc 弹起:tanqi:tq 弹跳:tantiao:tt 强劲:qiangjing:qj 归还:guihuan:gh 归降:guixiang:gx
当不了:dangbuliao:dbl 当差:dangchai:dc 彭长老:pengzhanglao:pzl 役畜:yichu:yc 徐乾学:xuganxue:xgx 徐行:xuhang:xh 徐长老:xuzhanglao:xzl 徐院长:xuyuanzhang:xyz
微缩:weisuo:ws 微薄:weibo:wb 微调:weitiao:wt 德莱塞:delaise:dls 心事重重:xinshichongchong:xscc 心惊胆颤:xinjingdanzhan:xjdz 心肌梗塞:xinjigengse:xjgs 忖度:cunduo:cd
忘不了:wangbuliao:wbl 念佛:nianfo:nf 怎么得了:zenmedeliao:zmdl 怎么着:zenmezhao:zmz 怨艾:yuanyi:yy 怪模怪样:guaimuguaiyang:gmgy 总务长:zongwuzhang:zwz 总参谋长:zongcanmouzhang:zcmz
总得:zongdei:zd 总称:zongcheng:zc 总行:zonghang:zh 总长:zongzhang:zz 总队长:zongduizhang:zdz 恐吓:konghe:kh 恐吓信:konghexin:khx 恫吓:donghe:dh
恶性疟:exingnue:exn 悄悄地:qiaoqiaode:qqd 情报局长:qingbaojuzhang:qbjz 慰藉:weijie:wj 憎恶:zengwu:zw 懂行:donghang:dh 懒觉:lanjiao:lj 戏称:xicheng:xc
成不了:chengbuliao:cbl 成佛:chengfo:cf 成长:chengzhang:cz 成长型:chengzhangxing:czx 成长性:chengzhangxing:czx 成长期:chengzhangqi:czq 戴校本:daijiaoben:djb 戴纶巾:daiguanjin:dgj
户长:huzhang:hz 所长:suozhang:sz 扁舟:pianzhou:pz 扎染:zaran:zr 扒手:pashou:ps 扒灰:pahui:ph 扒鸡:paji:pj 打击乐:dajiyue:djy
打击乐器:dajiyueqi:djyq 打工仔:dagongzai:dgz 打颤:dazhan:dz 扛鼎之作:gangdingzhizuo:gdzz 执拗:zhiniu:zn 执着:zhizhuo:zz 执著:zhizhuo:zz 执行长:zhixingzhang:zxz
抄没:chaomo:cm 投降:touxiang:tx 投降主义:touxiangzhuyi:txzy 投降书:touxiangshu:txs 投降派:touxiangpai:txp 抗疟:kangnue:kn 抗疟药:kangnueyao:kny 抛头露面:paotouloumian:ptlm
护士长:hushizhang:hsz 报称:baocheng:bc 抱佛脚:baofojiao:bfj 抱厦:baosha:bs 抹布:mabu:mb 抽咽:chouye:cy 抽血:chouxie:cx 拉纤:laqian:lq
拌和:banhuo:bh 拍卖行:paimaihang:pmh 拍手称快:paishouchengkuai:psck 拎包:linbao:lb 拓本:taben:tb 拓片:tapian:tp 拔苗助长:bamiaozhuzhang:bmzz 拗不过:niubuguo:nbg
拗陷:niuxian:nx 招商银行:zhaoshangyinhang:zsyh 招行:zhaohang:zh 招降:zhaoxiang:zx 拜佛:baifo:bf 拥塞:yongse:ys 拨给:bogei:bg 择菜:zhaicai:zc
拱券:gongxuan:gx 拾级:sheji:sj 拾级而上:shejiershang:sjes 拿给:nagei:ng 指称:zhicheng:zc 按辔徐行:anpeixuhang:apxh 挛缩:luansuo:ls 挺括:tinggua:tg
捆扎:kunza:kz 捐给:juangei:jg 捡便宜:jianpianyi:jpy 换血:huanxie:hx 换行:huanhang:hh 换行符:huanhangfu:hhf 据称:jucheng:jc 排行:paihang:ph
排行榜:paihangbang:phb 排长:paizhang:pz 掸邦:shanbang:sb 掺和:chanhuo:ch 提防:difang:df 揠苗助长:yamiaozhuzhang:ymzz 揣度:chuaiduo:cd 援藏:yuanzang:yz
搀和:chanhuo:ch 搅和:jiaohuo:jh 搜括:sougua:sg 搪塞:tangse:ts 摇滚乐:yaogunyue:ygy 摩天大厦:motiandasha:mtds 摩挲:mosuo:ms 摸得着:modezhao:mdz
攒动:cuandong:cd 攒眉:cuanmei:cm 支行:zhihang:zh 支队长:zhiduizhang:zdz 收缩:shousuo:ss 收缩压:shousuoya:ssy 收缩期:shousuoqi:ssq 收缩率:shousuolu:ssl
改称:gaicheng:gc 改行:gaihang:gh 放血:fangxie:fx 故伎重演:gujichongyan:gjcy 故地重游:gudichongyou:gdcy 故技重演:gujichongyan:gjcy 教务长:jiaowuzhang:jwz 教学相长:jiaoxuexiangzhang:jxxz
教给:jiaogei:jg 教育部长:jiaoyubuzhang:jybz 教育长:jiaoyuzhang:jyz 教长:jiaozhang:jz 敛声屏气:lianshengbingqi:lsbq 数传:shuzhuan:sz 数字模拟:shuzimuni:szmn 数得着:shudezhao:sdz
数行:shuhang:sh 数重:shuchong:sc 敷衍了事:fuyanliaoshi:fyls 敷衍塞责:fuyanseze:fysz 文传:wenzhuan:wz 文化部长:wenhuabuzhang:whbz 斜颈:xiejing:xj 斯再传:sizaizhuan:szz
斯忒藩:situifan:stf 斯率军:sishuaijun:ssj 新乐府:xinyuefu:xyf 新传:xinzhuan:xz 新闻部长:xinwenbuzhang:xwbz 方西传:fangxizhuan:fxz 施都丁:shidouding:sdd 旅团长:lutuanzhang:ltz
旅游局长:luyoujuzhang:lyjz 旅长:luzhang:lz 族长:zuzhang:zz 无的放矢:wudifangshi:wdfs 日本央行:ribenyanghang:rbyh 日本银行:ribenyinhang:rbyh 日薄西山:riboxishan:rbxs 旦角:danjue:dj
旧地重游:jiudichongyou:jdcy 昌废佛:changfeifo:cff 易传:yizhuan:yz 星宿:xingxiu:xx 昵称:nicheng:nc 智真长老:zhizhenzhanglao:zzzl 暖和:nuanhuo:nh 暖暖和和:nuannuanhuohuo:nnhh
暖暖地:nuannuande:nnd 曝光:baoguang:bg 曝光率:baoguanglu:bgl 曲长老:quzhanglao:qzl 曾世英:cengshiying:csy 曾业英:cengyeying:cyy 曾为楚:cengweichu:cwc 曾之宁:cengzhining:czn
曾令良:cenglingliang:cll 曾仲鸣:cengzhongming:czm 曾公亮:cenggongliang:cgl 曾几何时:cengjiheshi:cjhs 曾利明:cengliming:clm 曾剑秋:cengjianqiu:cjq 曾华锋:cenghuafeng:chf 曾启亮:cengqiliang:cql
曾图南:cengtunan:ctn 曾培炎:cengpeiyan:cpy 曾士楚:cengshichu:csc 曾宪林:cengxianlin:cxl 曾宪梓:cengxianzi:cxz 曾家庄:cengjiazhuang:cjz 曾尊固:cengzungu:czg 曾庆云:cengqingyun:cqy
曾庆红:cengqinghong:cqh 曾得明:cengdeming:cdm 曾思玉:cengsiyu:csy 曾率军:cengshuaijun:csj 曾用名:cengyongming:cym 曾省吾:cengshengwu:csw 曾答允:cengdayun:cdy 曾纪泽:cengjize:cjz
曾纪鸿:cengjihong:cjh 曾经:cengjing:cj 曾经沧海:cengjingcanghai:cjch 曾辛元:cengxinyuan:cxy 曾述及:cengshuji:csj 曾铁鸥:cengtieou:cto 月氏:yuezhi:yz 有朝一日:youzhaoyiri:yzyr
有模有样:youmuyouyang:ymyy 有的放矢:youdifangshi:ydfs 服务行业:fuwuhangye:fwhy 朝三暮四:zhaosanmusi:zsms 朝不保夕:zhaobubaoxi:zbbx 朝乾夕惕:zhaoqianxiti:zqxt 朝令夕改:zhaolingxigai:zlxg 朝夕:zhaoxi:zx
朝夕相处:zhaoxixiangchu:zxxc 朝思暮想:zhaosimuxiang:zsmx 朝日:zhaori:zr 朝朝暮暮:zhaozhaomumu:zzmm 朝歌:zhaoge:zg 朝气:zhaoqi:zq 朝气蓬勃:zhaoqipengbo:zqpb 朝秦暮楚:zhaoqinmuchu:zqmc
朝闻:zhaowen:zw 朝霞:zhaoxia:zx 朝露:zhaolu:zl 木模:mumu:mm 木管乐器:muguanyueqi:mgyq 未了:weiliao:wl 未曾:weiceng:wc 未雨绸缪:weiyuchoumou:wycm
末了:moliao:ml 本行:benhang:bh 本行业:benhangye:bhy 朱右曾:zhuyouceng:zyc 朱媚筠:zhumeiyun:zmy 朱重八:zhuchongba:zcb 朴东木:piaodongmu:pdm 朴刀:podao:pd
朴刀来:podaolai:pdl 朴利茅:piaolimao:plm 朴子内:piaozinei:pzn 朴定洙:piaodingzhu:pdz 朴成哲:piaochengzhe:pcz 朴智星:piaozhixing:pzx 朴次茅斯:piaocimaosi:pcms 朴永训:piaoyongxun:pyx
朴者和尚:piaozheheshang:pzhs 朴茨茅斯:piaocimaosi:pcms 朴达摩:piaodamo:pdm 机械行业:jixiehangye:jxhy 机长:jizhang:jz 杀出重围:shachuchongwei:sccw 杉木:shamu:sm 李一氓:liyimeng:lym
李嬷嬷:limomo:lmm 李昌镐:lichanghao:lch 李校书:lijiaoshu:ljs 李石曾:lishiceng:lsc 李适之:likuozhi:lkz 李道长:lidaozhang:ldz 村长:cunzhang:cz 杜长老:duzhanglao:dzl
杨廷筠:yangtingyun:yty 杨慎矜:yangshenjin:ysj 杨杏佛:yangxingfo:yxf 杨行密:yanghangmi:yhm 松筠庵:songyunan:sya 枕藉:zhenjie:zj 林长老:linzhanglao:lzl 枞树:congshu:cs
枸橼酸:juyuansuan:jys 柏拉图:bolatu:blt 柏林:bolin:bl 柏林墙:bolinqiang:blq 柏林市:bolinshi:bls 柑桔:ganju:gj 柔佛:roufo:rf 柞水县:zhashuixian:zsx
查伊璜:zhayihuang:zyh 查德威:zhadewei:zdw 查德森:zhadesen:zds 查志隆:zhazhilong:zzl 查慎行:zhashenxing:zsx 查继佐:zhajizuo:zjz 柳毅传:liuyizhuan:lyz 栅极:shanji:sj
标的:biaodi:bd 标的物:biaodiwu:bdw 标的额:biaodie:bde 标题音乐:biaotiyinyue:btyy 树碑立传:shubeilizhuan:sblz 栓塞:shuanse:ss 校准:jiaozhun:jz 校勘:jiaokan:jk
校勘学:jiaokanxue:jkx 校场:jiaochang:jc 校场口:jiaochangkou:jck 校对:jiaodui:jd 校本:jiaoben:jb 校样:jiaoyang:jy 校核:jiaohe:jh 校正:jiaozheng:jz
校注:jiaozhu:jz 校点:jiaodian:jd 校订:jiaoding:jd 校长:xiaozhang:xz 校阅:jiaoyue:jy 校验:jiaoyan:jy 核儿:huer:he 格列佛:geliefo:glf
桑葚:sangshen:ss 桔子:juzi:jz 桔红:juhong:jh 桔红色:juhongse:jhs 桔黄色:juhuangse:jhs 梁三长老:liangsanzhanglao:lszl 梁二长老:liangerzhanglao:lezl 梁长老:liangzhanglao:lzl
梗塞:gengse:gs 检察长:jianchazhang:jcz 椎体:chuiti:ct 槟榔:binglang:bl 模具:muju:mj 模具钢:mujugang:mjg 模子:muzi:mz 模板:muban:mb
模样:muyang:my 模样儿:muyanger:mye 横传:hengzhuan:hz 欠债还钱:qianzhaihuanqian:qzhq 次长:cizhang:cz 欧洲央行:ouzhouyanghang:ozyh 欧米茄:oumijia:omj 欲说还休:yushuohuanxiu:yshx
欺行霸市:qihangbashi:qhbs 款识:kuanzhi:kz 歌仔:gezai:gz 正传:zhengzhuan:zz 正着:zhengzhao:zz 正邪:zhengye:zy 此消彼长:cixiaobizhang:cxbz 武行:wuhang:wh
武装部长:wuzhuangbuzhang:wzbz 歪打正着:waidazhengzhao:wdzz 死不了:sibuliao:sbl 死死地:siside:ssd 殷红:yanhong:yh 母畜:muchu:mc 每行:meihang:mh 民乐:minyue:my
民政局长:minzhengjuzhang:mzjz 民生银行:minshengyinhang:msyh 民都洛:mindouluo:mdl 气势磅礴:qishipangbo:qspb 气喘吁吁:qichuanxuxu:qcxx 水泊:shuipo:sp 水泊梁山:shuipoliangshan:spls 水浒传:shuihuzhuan:shz
水萝卜:shuiluobo:slb 求神拜佛:qiushenbaifo:qsbf 汇丰银行:huifengyinhang:hfyh 汉藏语系:hanzangyuxi:hzyx 江竹筠:jiangzhuyun:jzy 汤汤水:shangshangshui:sss 汪校长:wangxiaozhang:wxz 沈曾植:shencengzhi:scz
沈沈叫:chenchenjiao:ccj 沉没:chenmo:cm 沉着:chenzhuo:cz 沙参:shashen:ss 沙咀:shazui:sz 没什么:meishenme:msm 没入:moru:mr 没奈何:monaihe:mnh
没完没了:meiwanmeiliao:mwml 没收:moshou:ms 没着没落:meizhemoluo:mzml 没药:moyao:my 没落:moluo:ml 没食子酸:moshizisuan:mszs 没齿不忘:mochibuwang:mcbw 没齿难忘:mochinanwang:mcnw
泄露:xielou:xl 泄露天机:xieloutianji:xltj 泛称:fancheng:fc 波罗的海:boluodihai:bldh 泰阿剑:taiejian:tej 洋行:yanghang:yh 活佛:huofo:hf 浅薄:qianbo:qb
浑身解数:hunshenxieshu:hsxs 浒墅关:xushuguan:xsg 浓缩:nongsuo:ns 浓缩铀:nongsuoyou:nsy 浙江广厦:zhejiangguangsha:zjgs 浚县:xunxian:xx 浦发银行:pufayinhang:pfyh 浴佛:yufo:yf
海参:haishen:hs 海参崴:haishenwai:hsw 浸没:jinmo:jm 消长:xiaozhang:xz 涡河:guohe:gh 淡淡地:dandande:ddd 淡薄:danbo:db 淤塞:yuse:ys
深恶痛绝:shenwutongjue:swtj 深深地:shenshende:ssd 淹没:yanmo:ym 清平乐:qingpingyue:qpy 渣打银行:zhadayinhang:zdyh 温庭筠:wentingyun:wty 温情脉脉:wenqingmomo:wqmm 游说:youshui:ys
湖泊:hupo:hp 湮没:yanmo:ym 湮没无闻:yanmowuwen:ymww 滋长:zizhang:zz 灵长目:lingzhangmu:lzm 灵长类:lingzhanglei:lzl 炊事班长:chuishibanzhang:csbz 炮烙:paoluo:pl
点着:dianzhao:dz 热胀冷缩:rezhanglengsuo:rzls 热轧:reya:ry 烹调:pengtiao:pt 烹调法:pengtiaofa:ptf 熊佛西:xiongfoxi:xfx 熨斗:yundou:yd 熨烫:yuntang:yt
爪子:zhuazi:zz 爱乐乐团:aiyueyuetuan:ayyt 爵士乐:jueshiyue:jsy 爵士乐队:jueshiyuedui:jsyd 片仔癀:pianzaihuang:pzh 牛仔:niuzai:nz 牛仔布:niuzaibu:nzb 牛仔服:niuzaifu:nzf
牛仔裤:niuzaiku:nzk 牛正乾:niuzhenggan:nzg 牛羊畜:niuyangchu:nyc 牟平:muping:mp 牲畜:shengchu:sc 牲畜头数:shengchutoushu:scts 犍为:qianwei:qw 犍为县:qianweixian:qwx
犯不着:fanbuzhao:fbz 犯得着:fandezhao:fdz 狂飚:kuangbiao:kb 狗仔队:gouzaidui:gzd 独辟蹊径:dupixijing:dpxj 狱长:yuzhang:yz 猛地:mengde:md 猜度:caiduo:cd
猪仔:zhuzai:zz 猪圈:zhujuan:zj 献给:xiangei:xg 献血:xianxie:xx 献血者:xianxiezhe:xxz 玄参:xuanshen:xs 玄参科:xuanshenke:xsk 率众:shuaizhong:sz
率先:shuaixian:sx 率先垂范:shuaixianchuifan:sxcf 率性:shuaixing:sx 率直:shuaizhi:sz 率真:shuaizhen:sz 率领:shuailing:sl 玉帝传:yudizhuan:ydz 王丙乾:wangbinggan:wbg
王乾娘:wangganniang:wgn 王体乾:wangtigan:wtg 王大夫:wangdaifu:wdf 王大珩:wangdaheng:wdh 王庭筠:wangtingyun:wty 王曰乾:wangyuegan:wyg 王胖子:wangpangzi:wpz 王蛤蟆:wanghama:whm
王辟光:wangpiguang:wpg 王述曾:wangshuceng:wsc 王道乾:wangdaogan:wdg 王重阳:wangchongyang:wcy 王镇长:wangzhenzhang:wzz 王队长:wangduizhang:wdz 王降汉:wangxianghan:wxh 环颈雉:huanjingzhi:hjz
现调机:xiantiaoji:xtj 玻色子:boshaizi:bsz 班组长:banzuzhang:bzz 班长:banzhang:bz 珲春:hunchun:hc 珲春市:hunchunshi:hcs 理事长:lishizhang:lsz 琢磨:zuomo:zm
琢磨不透:zuomobutou:zmbt 琴行:qinhang:qh 瑞士银行:ruishiyinhang:rsyh 瑟缩:sesuo:ss 瓜蔓:guawan:gw 瓦窑堡:wayaobu:wyb 瓶颈:pingjing:pj 甜甜地:tiantiande:ttd
生产队长:shengchanduizhang:scdz 生吞活剥:shengtunhuobo:sthb 生还:shenghuan:sh 生还者:shenghuanzhe:shz 生长:shengzhang:sz 生长期:shengzhangqi:szq 生长激素:shengzhangjisu:szjs 生长点:shengzhangdian:szd
生长素:shengzhangsu:szs 生长量:shengzhangliang:szl 用不着:yongbuzhao:ybz 田曾佩:tiancengpei:tcp 田长焯:tianchangchao:tcc 由召公:youshaogong:ysg 甲壳:jiaqiao:jq 甲壳动物:jiaqiaodongwu:jqdw
甲壳素:jiaqiaosu:jqs 电子音乐:dianziyinyue:dzyy 电熨斗:dianyundou:dyd 电解池:dianxiechi:dxc 画传:huazhuan:hz 畏畏缩缩:weiweisuosuo:wwss 畏缩:weisuo:ws 畏缩不前:weisuobuqian:wsbq
留给:liugei:lg 畜力:chuli:cl 畜牲:chusheng:cs 畜生:chusheng:cs 畜禽:chuqin:cq 畜类:chulei:cl 畜群:chuqun:cq 畜舍:chushe:cs
番禺:panyu:py 番禺区:panyuqu:pyq 番禺县:panyuxian:pyx 番禺市:panyushi:pys 疟原虫:nueyuanchong:nyc 疟疾:nueji:nj 疯长:fengzhang:fz 痴痴地:chichide:ccd
瘠薄:jibo:jb 瘦削:shouxue:sx 癞蛤蟆:laihama:lhm 白术:baizhu:bz 白术散:baizhusan:bzs 白白地:baibaide:bbd 白白胖胖:baibaipangpang:bbpp 白萝卜:bailuobo:blb
白蛇传:baishezhuan:bsz 白长老:baizhanglao:bzl 的哥:dige:dg 的士:dishi:ds 的的确确:didiqueque:ddqq 的确:dique:dq 的确如此:diqueruci:dqrc 的确良:diqueliang:dql
的黎波里:diliboli:dlbl 皱缩:zhousuo:zs 监狱长:jianyuzhang:jyz 盘剥:panbo:pb 盘诘:panjie:pj 盛饭:chengfan:cf 目无尊长:muwuzunzhang:mwzz 目的:mudi:md
目的地:mudidi:mdd 目的性:mudixing:mdx 目的论:mudilun:mdl 直传:zhizhuan:zz 直截了当:zhijieliaodang:zjld 直接了当:zhijieliaodang:zjld 直率:zhishuai:zs 直直地:zhizhide:zzd
相率:xiangshuai:xs 省亲:xingqin:xq 省察:xingcha:xc 省悟:xingwu:xw 省视:xingshi:xs 省长:shengzhang:sz 着凉:zhaoliang:zl 着力:zhuoli:zl
着力点:zhuolidian:zld 着地:zhuodi:zd 着墨:zhuomo:zm 着实:zhuoshi:zs 着床:zhuochuang:zc 着急:zhaoji:zj 着想:zhuoxiang:zx 着意:zhuoyi:zy
着手:zhuoshou:zs 着手成春:zhuoshouchengchun:zscc 着数:zhaoshu:zs 着法:zhaofa:zf 着火:zhaohuo:zh 着眼:zhuoyan:zy 着眼于:zhuoyanyu:zyy 着眼点:zhuoyandian:zyd
着着实实:zhezhuoshishi:zzss 着色:zhuose:zs 着色剂:zhuoseji:zsj 着落:zhuoluo:zl 着装:zhuozhuang:zz 着迷:zhaomi:zm 着重:zhuozhong:zz 着重点:zhuozhongdian:zzd
着陆:zhuolu:zl 着陆器:zhuoluqi:zlq 着陆点:zhuoludian:zld 着魔:zhaomo:zm 睡着:shuizhao:sz 睡觉:shuijiao:sj 督率:dushuai:ds 矜持:jinchi:jc
短不了:duanbuliao:dbl 矮胖:aipang:ap 石佛寺:shifosi:sfs 石油部长:shiyoubuzhang:sybz 矿长:kuangzhang:kz 破镜重圆:pojingchongyuan:pjcy 硬着陆:yingzhuolu:yzl 碌碡:liuzhou:lz
磅礴:pangbo:pb 磨削:moxue:mx 礼乐:liyue:ly 礼佛:lifo:lf 礼崩乐坏:libengyuehuai:lbyh 社长:shezhang:sz 神佛:shenfo:sf 神出鬼没:shenchuguimo:scgm
神差鬼使:shenchaiguishi:scgs 禅让:shanrang:sr 禅让制:shanrangzhi:srz 禽畜:qinchu:qc 种姓:chongxing:cx 种畜场:zhongchuchang:zcc 科长:kezhang:kz 秘书长:mishuzhang:msz
秘鲁:bilu:bl 秘鲁人:biluren:blr 秘鲁政府:biluzhengfu:blzf 租给:zugei:zg 秦校长:qinxiaozhang:qxz 秦桧:qinhui:qh 秦桧制造:qinhuizhizao:qhzz 秦桧死:qinhuisi:qhs
称为:chengwei:cw 称之为:chengzhiwei:czw 称作:chengzuo:cz 称做:chengzuo:cz 称兄道弟:chengxiongdaodi:cxdd 称号:chenghao:ch 称呼:chenghu:ch 称奇:chengqi:cq
称孤道寡:chenggudaogua:cgdg 称帝:chengdi:cd 称得上:chengdeshang:cds 称得起:chengdeqi:cdq 称扬:chengyang:cy 称法:chengfa:cf 称王:chengwang:cw 称王称霸:chengwangchengba:cwcb
称病:chengbing:cb 称羡:chengxian:cx 称臣:chengchen:cc 称誉:chengyu:cy 称许:chengxu:cx 称谓:chengwei:cw 称谢:chengxie:cx 称赏:chengshang:cs
称赞:chengzan:cz 称道:chengdao:cd 称重:chengzhong:cz 称量:chengliang:cl 称雄:chengxiong:cx 称霸:chengba:cb 称颂:chengsong:cs 稀薄:xibo:xb
税务所长:shuiwusuozhang:swsz 稳稳地:wenwende:wwd 稽首:qishou:qs 穆棱:muling:ml 穆棱河:mulinghe:mlh 空落落:konglaolao:kll 空调:kongtiao:kt 空调器:kongtiaoqi:ktq
空调机:kongtiaoji:ktj 穿着:chuanzhuo:cz 立传:lizhuan:lz 立地成佛:lidichengfo:ldcf 站长:zhanzhang:zz 端的:duandi:dd 第一行:diyihang:dyh 第一重:diyichong:dyc
第三重:disanchong:dsc 第二行:dierhang:deh 简洁明了:jianjiemingliao:jjml 简称:jiancheng:jc 简长老:jianzhanglao:jzl 算不了:suanbuliao:sbl 管乐:guanyue:gy 管乐器:guanyueqi:gyq
管乐队:guanyuedui:gyd 管弦乐:guanxianyue:gxy 管弦乐器:guanxianyueqi:gxyq 管弦乐团:guanxianyuetuan:gxyt 管弦乐曲:guanxianyuequ:gxyq 管弦乐队:guanxianyuedui:gxyd 箪食壶浆:dansihujiang:dshj 籍没:jimo:jm
米芾:mifu:mf 粗率:cushuai:cs 粘乎乎:zhanhuhu:zhh 粘多糖:zhanduotang:zdt 粘弹性:niantanxing:ntx 粘接:zhanjie:zj 粘滞:zhanzhi:zz 粘滞性:zhanzhixing:zzx
粘着:nianzhuo:nz 粘糊糊:zhanhuhu:zhh 粘虫:zhanchong:zc 粘贴:zhantie:zt 粘连:zhanlian:zl 精辟:jingpi:jp 糖色:tangshai:ts 糜子:meizi:mz
系带:jidai:jd 系统地:xitongde:xtd 素称:sucheng:sc 紧缩:jinsuo:js 紧缩性:jinsuoxing:jsx 繁峙:fanshi:fs 繁峙县:fanshixian:fsx 红萝卜:hongluobo:hlb
红颜薄命:hongyanboming:hybm 纤夫:qianfu:qf 纤手:qianshou:qs 纤绳:qiansheng:qs 纪传体:jizhuanti:jzt 纰缪:pimiu:pm 纳降:naxiang:nx 纶巾羽扇:guanjinyushan:gjys
组织部长:zuzhibuzhang:zzbz 组长:zuzhang:zz 细细地:xixide:xxd 终了:zhongliao:zl 经传:jingzhuan:jz 经济部长:jingjibuzhang:jjbz 绑扎:bangza:bz 结扎:jieza:jz
给出:geichu:gc 给定:geiding:gd 给钱:geiqian:gq 给面子:geimianzi:gmz 络子:laozi:lz 统战部长:tongzhanbuzhang:tzbz 统率:tongshuai:ts 统称:tongcheng:tc
绵薄:mianbo:mb 绸缪:choumou:cm 缩写:suoxie:sx 缩减:suojian:sj 缩印本:suoyinben:syb 缩回:suohui:sh 缩头:suotou:st 缩头缩脑:suotousuonao:stsn
缩小:suoxiao:sx 缩影:suoying:sy 缩微:suowei:sw 缩微胶片:suoweijiaopian:swjp 缩成一团:suochengyituan:scyt 缩手:suoshou:ss 缩手缩脚:suoshousuojiao:sssj 缩水:suoshui:ss
缩略语:suolueyu:sly 缩短:suoduan:sd 缩紧:suojin:sj 缩编:suobian:sb 缩聚:suoju:sj 缩脖子:suobozi:sbz 缩进:suojin:sj 缩醛:suoquan:sq
缺血:quexie:qx 缺血性:quexiexing:qxx 罗刹:luocha:lc 罗布泊:luobupo:lbp 罗布泊湖:luobupohu:lbph 罚没:famo:fm 罢了:baliao:bl 署长:shuzhang:sz
羊圈:yangjuan:yj 美人蕉:meirenjiao:mrj 美国银行:meiguoyinhang:mgyh 美的:meidi:md 美称:meicheng:mc 群氓:qunmeng:qm 羽扇纶巾:yushanguanjin:ysgj 翟俊杰:zhaijunjie:zjj
翟廷玉:zhaitingyu:zty 翟彦鹏:zhaiyanpeng:zyp 翟振华:zhaizhenhua:zzh 翟景升:zhaijingsheng:zjs 翟理斯:zhailisi:zls 翟秋白:zhaiqiubai:zqb 翟赋明:zhaifuming:zfm 老佛爷:laofoye:lfy
老大夫:laodaifu:ldf 老所长:laosuozhang:lsz 老本行:laobenhang:lbh 老调重弹:laodiaochongtan:ldct 耕畜:gengchu:gc 耙子:pazi:pz 职称:zhicheng:zc 联队长:lianduizhang:ldz
肋条:leitiao:lt 肋骨:leigu:lg 肖院长:xiaoyuanzhang:xyz 股长:guzhang:gz 肥差:feichai:fc 肥胖:feipang:fp 肥胖病:feipangbing:fpb 肥胖症:feipangzheng:fpz
肥胖者:feipangzhe:fpz 背景音乐:beijingyinyue:bjyy 胖乎乎:panghuhu:phh 胖嘟嘟:pangdudu:pdd 胖墩墩:pangdundun:pdd 胖大海:pangdahai:pdh 胖头鱼:pangtouyu:pty 胖子:pangzi:pz
胖尊者:pangzunzhe:pzz 胖小子:pangxiaozi:pxz 胖瘦:pangshou:ps 胖胖:pangpang:pp 胖胖的:pangpangde:ppd 胡佛:hufo:hf 胡佛坝:hufoba:hfb 胡孝乾:huxiaogan:hxg
胡校长:huxiaozhang:hxz 胡萝卜:huluobo:hlb 胡萝卜素:huluobosu:hlbs 胳肢窝:gazhiwo:gzw 胳臂:gebei:gb 胶着:jiaozhuo:jz 胶着状态:jiaozhuozhuangtai:jzzt 胶粘:jiaozhan:jz
胶粘剂:jiaozhanji:jzj 胸脯:xiongpu:xp 能源部长:nengyuanbuzhang:nybz 脉脉:momo:mm 脉脉含情:momohanqing:mmhq 脑出血:naochuxie:ncx 脑栓塞:naoshuanse:nss 脚色:juese:js
脱不了身:tuobuliaoshen:tbls 腌制:yanzhi:yz 腌渍:yanzi:yz 腌肉:yanrou:yr 腌菜:yancai:yc 腌鱼:yanyu:yy 膀胱:pangguang:pg 膀胱炎:pangguangyan:pgy
膀胱癌:pangguangai:pga 臧否人物:zangpirenwu:zprw 自传:zizhuan:zz 自传体:zizhuanti:zzt 自怨自艾:ziyuanziyi:zyzy 自省:zixing:zx 自称:zicheng:zc 自称为:zichengwei:zcw
般地:bande:bd 般若:bore:br 舰长:jianzhang:jz 船长:chuanzhang:cz 船长室:chuanzhangshi:czs 艇长:tingzhang:tz 节衣缩食:jieyisuoshi:jyss 芫荽:yansui:ys
芭蕉:bajiao:bj 芭蕉叶:bajiaoye:bjy 芭蕉扇:bajiaoshan:bjs 花呢:huani:hn 花旗银行:huaqiyinhang:hqyh 花落谁家:hualuosheijia:hlsj 苍劲:cangjing:cj 苍术:cangzhu:cz
苎麻:zhuma:zm 苦差:kuchai:kc 苦差事:kuchaishi:kcs 苯并芘:benbingbi:bbb 英雄传:yingxiongzhuan:yxz 茁壮成长:zhuozhuangchengzhang:zzcz 范佛里:fanfoli:ffl 范大夫:fandaifu:fdf
茅塞:maose:ms 茅塞顿开:maosedunkai:msdk 草率:caoshuai:cs 草草了事:caocaoliaoshi:ccls 草长莺飞:caozhangyingfei:czyf 荨麻疹:xunmazhen:xmz 荷兰银行:helanyinhang:hlyh 荸荠:biqi:bq
莎草:suocao:sc 莎草科:suocaoke:sck 莞尔:waner:we 莞尔一笑:waneryixiao:weyx 莫朴树:moposhu:mps 莫邪:moye:my 莫长老:mozhanglao:mzl 莲花落:lianhualao:lhl
菲薄:feibo:fb 萎缩:weisuo:ws 萎缩性:weisuoxing:wsx 萝卜:luobo:lb 萝卜丝:luobosi:lbs 萝卜干:luobogan:lbg 萝卜花:luobohua:lbh 营长:yingzhang:yz
萧史乘:xiaoshisheng:xss 落枕:laozhen:lz 著称:zhucheng:zc 葛长老:gezhanglao:gzl 董事长:dongshizhang:dsz 葫芦蔓:huluwan:hlw 葫蔓藤:huwanteng:hwt 蓝道行:landaoheng:ldh
蔚县:yuxian:yx 蔚州:yuzhou:yz 蕉麻:jiaoma:jm 蕴藉:yunjie:yj 薄利多销:boliduoxiao:bldx 薄命:boming:bm 薄幸:boxing:bx 薄幸之:boxingzhi:bxz
薄弱:boruo:br 薄弱校:boruoxiao:brx 薄弱点:boruodian:brd 薄情:boqing:bq 薄技:boji:bj 薄暮:bomu:bm 薄熙来:boxilai:bxl 薄礼:boli:bl
薄膜:bomo:bm 薄荷:bohe:bh 薄荷油:boheyou:bhy 薄荷糖:bohetang:bht 薄荷脑:bohenao:bhn 薄荷醇:bohechun:bhc 薄薄地:baobaode:bbd 薄被:bobei:bb
薄酒:bojiu:bj 薄雾:bowu:bw 藉藉无名:jiejiewuming:jjwm 藏传:cangzhuan:cz 藏刀:zangdao:zd 藏北:zangbei:zb 藏区:zangqu:zq 藏医:zangyi:zy
藏医学:zangyixue:zyx 藏历:zangli:zl 藏学:zangxue:zx 藏学家:zangxuejia:zxj 藏式:zangshi:zs 藏戏:zangxi:zx 藏文:zangwen:zw 藏族:zangzu:zz
藏族人:zangzuren:zzr 藏民:zangmin:zm 藏王:zangwang:zw 藏红花:zanghonghua:zhh 藏羚:zangling:zl 藏羚羊:zanglingyang:zly 藏药:zangyao:zy 藏语:zangyu:zy
藏语文:zangyuwen:zyw 藏青:zangqing:zq 藏青色:zangqingse:zqs 藏香:zangxiang:zx 藤蔓:tengwan:tw 虚与委蛇:xuyuweiyi:xywy 虾蟆:hama:hm 蚌埠:bengbu:bb
蚌埠市:bengbushi:bbs 蛤蟆:hama:hm 蛤蟆镜:hamajing:hmj 蜜里调油:militiaoyou:mlty 蜷缩:quansuo:qs 血块:xiekuai:xk 血泊:xuepo:xp 血淋淋:xielinlin:xll
血糊糊:xiehuhu:xhh 行业:hangye:hy 行业性:hangyexing:hyx 行伍:hangwu:hw 行会:hanghui:hh 行内:hangnei:hn 行列:hanglie:hl 行列式:hanglieshi:hls
行号:hanghao:hh 行商:hangshang:hs 行家:hangjia:hj 行家里手:hangjialishou:hjls 行市:hangshi:hs 行当:hangdang:hd 行情:hangqing:hq 行款:hangkuan:hk
行规:hanggui:hg 行话:hanghua:hh 行货:hanghuo:hh 行距:hangju:hj 行辈:hangbei:hb 行长:hangzhang:hz 行间:hangjian:hj 衣着:yizhuo:yz
衣锦还乡:yijinhuanxiang:yjhx 表率:biaoshuai:bs 袅娜:niaonuo:nn 袅袅娜娜:niaoniaonuonuo:nnnn 装帧:zhuangzhen:zz 装模作样:zhuangmuzuoyang:zmzy 裙带关系:qundaiguanji:qdgj 裹扎:guoza:gz
褚劲风:chujingfeng:cjf 西柏林:xibolin:xbl 西洋参:xiyangshen:xys 西藏:xizang:xz 西藏地区:xizangdiqu:xzdq 西藏大学:xizangdaxue:xzdx 西藏天路:xizangtianlu:xztl 西藏药业:xizangyaoye:xzyy
西藏路:xizanglu:xzl 西藏高原:xizanggaoyuan:xzgy 西行长:xihangzhang:xhz 西道行:xidaoheng:xdh 要不了:yaobuliao:ybl 覆没:fumo:fm 见称:jiancheng:jc 角儿:jueer:je
角力:jueli:jl 角抵:juedi:jd 角斗:juedou:jd 角斗场:juedouchang:jdc 角斗士:juedoushi:jds 角色:juese:js 角逐:juezhu:jz 解宝:xiebao:xb
解数:xieshu:xs 解文豹:xiewenbao:xwb 解调:jietiao:jt 言伯乾:yanbogan:ybg 言归正传:yanguizhengzhuan:ygzz 警长:jingzhang:jz 讨价还价:taojiahuanjia:tjhj 讨便宜:taopianyi:tpy
讨还:taohuan:th 让给:ranggei:rg 议长:yizhang:yz 许寿裳:xushouchang:xsc 许德珩:xudeheng:xdh 评传:pingzhuan:pz 评弹:pingtan:pt 诈称:zhacheng:zc
诈降:zhaxiang:zx 诗行:shihang:sh 诘问:jiewen:jw 该行:gaihang:gh 语塞:yuse:ys 诱降:youxiang:yx 说不着:shuobuzhao:sbz 说得着:shuodezhao:sdz
诸乐调:zhuyuediao:zyd 诸道行:zhudaoheng:zdh 诸长老:zhuzhanglao:zzl 课长:kezhang:kz 调价:tiaojia:tj 调休:tiaoxiu:tx 调低:tiaodi:td 调侃:tiaokan:tk
调停:tiaoting:tt 调停人:tiaotingren:ttr 调停者:tiaotingzhe:ttz 调养:tiaoyang:ty 调减:tiaojian:tj 调制:tiaozhi:tz 调制器:tiaozhiqi:tzq 调剂:tiaoji:tj
调匀:tiaoyun:ty 调压:tiaoya:ty 调合:tiaohe:th 调味:tiaowei:tw 调味剂:tiaoweiji:twj 调味品:tiaoweipin:twp 调味品厂:tiaoweipinchang:twpc 调味料:tiaoweiliao:twl
调和:tiaohe:th 调和主义:tiaohezhuyi:thzy 调和阴阳:tiaoheyinyang:thyy 调唆:tiaosuo:ts 调处:tiaochu:tc 调幅:tiaofu:tf 调弄:tiaonong:tn 调情:tiaoqing:tq
调戏:tiaoxi:tx 调控:tiaokong:tk 调摄:tiaoshe:ts 调教:tiaojiao:tj 调整:tiaozheng:tz 调整期:tiaozhengqi:tzq 调整法:tiaozhengfa:tzf 调料:tiaoliao:tl
调治:tiaozhi:tz 调测:tiaoce:tc 调理:tiaoli:tl 调皮:tiaopi:tp 调相:tiaoxiang:tx 调笑:tiaoxiao:tx 调经:tiaojing:tj 调羹:tiaogeng:tg
调色:tiaose:ts 调色板:tiaoseban:tsb 调节:tiaojie:tj 调节价:tiaojiejia:tjj 调节剂:tiaojieji:tjj 调节器:tiaojieqi:tjq 调节税:tiaojieshui:tjs 调蓄:tiaoxu:tx
调解:tiaojie:tj 调解书:tiaojieshu:tjs 调解人:tiaojieren:tjr 调试:tiaoshi:ts 调谐:tiaoxie:tx 调适:tiaoshi:ts 调速:tiaosu:ts 调速器:tiaosuqi:tsq
调配:tiaopei:tp 调酒:tiaojiu:tj 调酒师:tiaojiushi:tjs 调音:tiaoyin:ty 调音师:tiaoyinshi:tys 调频:tiaopin:tp 调香:tiaoxiang:tx 谎称:huangcheng:hc
谐调:xietiao:xt 贝大夫:beidaifu:bdf 财会:caikuai:ck 财政部长:caizhengbuzhang:czbz 财长:caizhang:cz 败给:baigei:bg 贪便宜:tanpianyi:tpy 贾人达:gurenda:grd
贾平凹:jiapingwa:jpw 贾长老:jiazhanglao:jzl 赔还:peihuan:ph 赖嬷嬷:laimomo:lmm 赠给:zenggei:zg 赵大夫:zhaodaifu:zdf 赵嬷嬷:zhaomomo:zmm 赵家堡:zhaojiapu:zjp
起落架:qilaojia:qlj 越长越:yuezhangyue:yzy 趔趄:lieqie:lq 趔趔趄趄:lielieqieqie:llqq 跑马卖解:paomamaixie:pmmx 蹊径:xijing:xj 身单力薄:shendanlibo:sdlb 身着:shenzhuo:sz
躯壳:quqiao:qq 车行:chehang:ch 车马炮:jumapao:jmp 轧死:yasi:ys 轧花:yahua:yh 轧花厂:yahuachang:yhc 轧花机:yahuaji:yhj 转给:zhuangei:zg
转行:zhuanhang:zh 软和:ruanhuo:rh 软着陆:ruanzhuolu:rzl 轴颈:zhoujing:zj 轻徭薄赋:qingyaobofu:qybf 轻率:qingshuai:qs 轻薄:qingbo:qb 轻薄无行:qingbowuxing:qbwx
轻轻地:qingqingde:qqd 轻音乐:qingyinyue:qyy 载畜量:zaichuliang:zcl 输给:shugei:sg 辟谣:piyao:py 辩称:biancheng:bc 辱没:rumo:rm 边路传:bianluzhuan:blz
返老还童:fanlaohuantong:flht 返还:fanhuan:fh 还乡:huanxiang:hx 还乡团:huanxiangtuan:hxt 还价:huanjia:hj 还俗:huansu:hs 还债:huanzhai:hz 还击:huanji:hj
还原:huanyuan:hy 还原剂:huanyuanji:hyj 还原性:huanyuanxing:hyx 还原法:huanyuanfa:hyf 还原论:huanyuanlun:hyl 还嘴:huanzui:hz 还愿:huanyuan:hy 还我河山:huanwoheshan:hwhs
还手:huanshou:hs 还政于民:huanzhengyumin:hzym 还款:huankuan:hk 还款期:huankuanqi:hkq 还款额:huankuane:hke 还清:huanqing:hq 还珠格格:huanzhugege:hzgg 还礼:huanli:hl
还给:huangei:hg 还贷:huandai:hd 还阳:huanyang:hy 还魂:huanhun:hh 还魂草:huanhuncao:hhc 这么着:zhemezhao:zmz 进藏:jinzang:jz 远涉重洋:yuanshechongyang:yscy
远渡重洋:yuanduchongyang:ydcy 远远地:yuanyuande:yyd 远隔重洋:yuangechongyang:ygcy 连胖子:lianpangzi:lpz 连长:lianzhang:lz 迫击炮:paijipao:pjp 退缩:tuisuo:ts 退耕还林:tuigenghuanlin:tghl
退还:tuihuan:th 送给:songgei:sg 送还:songhuan:sh 递给:digei:dg 通称:tongcheng:tc 通货紧缩:tonghuojinsuo:thjs 造血:zaoxie:zx 遒劲:qiujing:qj
道行:daoheng:dh 道长:daozhang:dz 那罗刹:naluocha:nlc 那长老:nazhanglao:nzl 邮差:youchai:yc 郑长老:zhengzhanglao:zzl 郦食其:liyiji:lyj 部长:buzhang:bz
部长会议:buzhanghuiyi:bzhy 部长级:buzhangji:bzj 部队长:buduizhang:bdz 郭振乾:guozhengan:gzg 郭靖曾:guojingceng:gjc 都乐:doule:dl 都大锦:doudajin:ddj 都建康:doujiankang:djk
都必领:doubiling:dbl 都悔青:douhuiqing:dhq 都拉斯:doulasi:dls 都柏林:dubolin:dbl 都柳江:douliujiang:dlj 都江:doujiang:dj 都洛邑:douluoyi:dly 都能安:dounengan:dna
都行:douxing:dx 都诺夫:dounuofu:dnf 鄙薄:bibo:bb 酋长:qiuzhang:qz 酋长国:qiuzhangguo:qzg 配乐:peiyue:py 配角:peijue:pj 酒酿:jiuniang:jn
酝酿:yunniang:yn 酬酢:chouzuo:cz 酿制:niangzhi:nz 酿成:niangcheng:nc 酿造:niangzao:nz 酿造业:niangzaoye:nzy 酿酒:niangjiu:nj 酿酒业:niangjiuye:njy
酿酒厂:niangjiuchang:njc 酿酒师:niangjiushi:njs 里弄:lilong:ll 里长:lizhang:lz 重九:chongjiu:cj 重修:chongxiu:cx 重修旧好:chongxiujiuhao:cxjh 重光:chongguang:cg
重写:chongxie:cx 重出:chongchu:cc 重印:chongyin:cy 重印本:chongyinben:cyb 重叠:chongdie:cd 重叠式:chongdieshi:cds 重合:chonghe:ch 重名:chongming:cm
重启:chongqi:cq 重唱:chongchang:cc 重回:chonghui:ch 重围:chongwei:cw 重塑:chongsu:cs 重复:chongfu:cf 重复性:chongfuxing:cfx 重奏:chongzou:cz
重婚:chonghun:ch 重婚案:chonghunan:cha 重婚罪:chonghunzui:chz 重孙:chongsun:cs 重孙子:chongsunzi:csz 重审:chongshen:cs 重山:chongshan:cs 重峦叠嶂:chongluandiezhang:cldz
重庆:chongqing:cq 重庆地区:chongqingdiqu:cqdq 重庆大学:chongqingdaxue:cqdx 重庆市:chongqingshi:cqs 重庆队:chongqingdui:cqd 重建:chongjian:cj 重开:chongkai:ck 重归于好:chongguiyuhao:cgyh
重影:chongying:cy 重振:chongzhen:cz 重振旗鼓:chongzhenqigu:czqg 重排:chongpai:cp 重提:chongti:ct 重播:chongbo:cb 重操旧业:chongcaojiuye:ccjy 重整:chongzheng:cz
重整旗鼓:chongzhengqigu:czqg 重新:chongxin:cx 重构:chonggou:cg 重檐:chongyan:cy 重氮:chongdan:cd 重氮化:chongdanhua:cdh 重温:chongwen:cw 重演:chongyan:cy
重现:chongxian:cx 重生:chongsheng:cs 重生父母:chongshengfumu:csfm 重申:chongshen:cs 重登:chongdeng:cd 重组:chongzu:cz 重置:chongzhi:cz 重耳:chonger:ce
重见:chongjian:cj 重见天日:chongjiantianri:cjtr 重言:chongyan:cy 重设:chongshe:cs 重读:chongdu:cd 重起炉灶:chongqiluzao:cqlz 重蹈:chongdao:cd 重蹈覆辙:chongdaofuzhe:cdfz
重返:chongfan:cf 重逢:chongfeng:cf 重重:chongchong:cc 重重包围:chongchongbaowei:ccbw 重重叠叠:chongchongdiedie:ccdd 重重围困:chongchongweikun:ccwk 重重地:zhongzhongde:zzd 重重的:chongchongde:ccd
重阳:chongyang:cy 重阳节:chongyangjie:cyj 金佛山:jinfoshan:jfs 金兀术:jinwuzhu:jwz 金学曾:jinxueceng:jxc 金桔:jinju:jj 金称臣:jinchengchen:jcc 金蝉脱壳:jinchantuoqiao:jctq
金蝉长老:jinchanzhanglao:jczl 金钥匙:jinyaoshi:jys 金面佛:jinmianfo:jmf 钥匙:yaoshi:ys 钥匙圈:yaoshiquan:ysq 钥匙孔:yaoshikong:ysk 钦差:qinchai:qc 钦差大臣:qinchaidachen:qcdc
钱校本:qianjiaoben:qjb 铁佛寺:tiefosi:tfs 铃铛:lingdang:ld 铅山:yanshan:ys 铅山县:yanshanxian:ysx 铜管乐:tongguanyue:tgy 铜管乐器:tongguanyueqi:tgyq 铜臭:tongxiu:tx
银行:yinhang:yh 银行业:yinhangye:yhy 银行券:yinhangquan:yhq 银行卡:yinhangka:yhk 银行学:yinhangxue:yhx 银行家:yinhangjia:yhj 银行法:yinhangfa:yhf 银行界:yinhangjie:yhj
锒铛入狱:langdangruyu:ldry 键盘乐器:jianpanyueqi:jpyq 镇长:zhenzhang:zz 镜泊湖:jingpohu:jph 长一智:zhangyizhi:zyz 长上:zhangshang:zs 长传:changzhuan:cz 长兄:zhangxiong:zx
长势:zhangshi:zs 长史:zhangshi:zs 长吁短叹:changxuduantan:cxdt 长大:zhangda:zd 长大成人:zhangdachengren:zdcr 长女:zhangnu:zn 长子:zhangzi:zz 长孙:zhangsun:zs
长官:zhangguan:zg 长官司:zhangguansi:zgs 长幼慈:zhangyouci:zyc 长幼有序:zhangyouyouxu:zyyx 长成:zhangcheng:zc 长机:zhangji:zj 长毛:zhangmao:zm 长毛兔:zhangmaotu:zmt
长毛绒:zhangmaorong:zmr 长满:zhangman:zm 长相:zhangxiang:zx 长老:zhanglao:zl 长老会:zhanglaohui:zlh 长老派:zhanglaopai:zlp 长者:zhangzhe:zz 长辈:zhangbei:zb
长进:zhangjin:zj 长长地:changchangde:ccd 长颈鹿:changjinglu:cjl 门槛:menkan:mk 门槛儿:menkaner:mke 闪闪地:shanshande:ssd 闭塞:bise:bs 闭目塞听:bimuseting:bmst
闵行区:minhangqu:mhq 闵行校:minhangxiao:mhx 闷闷地:menmende:mmd 阎大夫:yandaifu:ydf 阏氏:yanzhi:yz 阚维雍:kanweiyong:kwy 队长:duizhang:dz 阻塞:zuse:zs
阻塞性:zusexing:zsx 阿佛洛:afoluo:afl 阿尔忒:aertui:aet 阿弥陀:emituo:emt 阿弥陀佛:emituofo:emtf 阿房宫:epanggong:epg 阿房宫赋:epanggongfu:epgf 阿胶:ejiao:ej
阿谀:eyu:ey 阿谀奉承:eyufengcheng:eyfc 阿谀逢迎:eyufengying:eyfy 附着:fuzhuo:fz 附着力:fuzhuoli:fzl 附着物:fuzhuowu:fzw 陈之佛:chenzhifo:czf 陈重名:chenchongming:ccm
陈镐民:chenhaomin:chm 陈长老:chenzhanglao:czl 降伏:xiangfu:xf 降服:xiangfu:xf 降顺:xiangshun:xs 降龙伏虎:xianglongfuhu:xlfh 院长:yuanzhang:yz 陷没:xianmo:xm
随行就市:suihangjiushi:shjs 隐没:yinmo:ym 雁行:yanhang:yh 雄劲:xiongjing:xj 雅乐:yayue:yy 雪茄:xuejia:xj 雪茄烟:xuejiayan:xjy 霓裳:nichang:nc
露一手:louyishou:lys 露头:loutou:lt 露脸:loulian:ll 露面:loumian:lm 露馅:louxian:lx 露马脚:loumajiao:lmj 青灯古佛:qingdenggufo:qdgf 青菜萝卜:qingcailuobo:qclb
青藏:qingzang:qz 青藏公路:qingzanggonglu:qzgl 青藏铁路:qingzangtielu:qztl 青藏高原:qingzanggaoyuan:qzgy 静静地:jingjingde:jjd 非得:feidei:fd 非银行:feiyinhang:fyh 鞭辟入里:bianpiruli:bprl
音乐:yinyue:yy 音乐会:yinyuehui:yyh 音乐剧:yinyueju:yyj 音乐厅:yinyueting:yyt 音乐史:yinyueshi:yys 音乐堂:yinyuetang:yyt 音乐声:yinyuesheng:yys 音乐季:yinyueji:yyj
音乐学:yinyuexue:yyx 音乐学院:yinyuexueyuan:yyxy 音乐家:yinyuejia:yyj 音乐性:yinyuexing:yyx 音乐感:yinyuegan:yyg 音乐指导:yinyuezhidao:yyzd 音乐片:yinyuepian:yyp 音乐界:yinyuejie:yyj
音乐系:yinyuexi:yyx 音乐节:yinyuejie:yyj 音乐课:yinyueke:yyk 须得先:xudeixian:xdx 顾不了:gubuliao:gbl 顾虑重重:guluchongchong:glcc 顾颉刚:guxiegang:gxg 颈侧:jingce:jc
颈内:jingnei:jn 颈椎:jingzhui:jz 颈椎病:jingzhuibing:jzb 颈肩痛:jingjiantong:jjt 颈脖:jingbo:jb 颈部:jingbu:jb 颈项:jingxiang:jx 颉颃:xiehang:xh
额手称庆:eshouchengqing:escq 颤栗:zhanli:zl 风调雨顺:fengtiaoyushun:ftys 风雨剥蚀:fengyuboshi:fybs 馆长:guanzhang:gz 首长:shouzhang:sz 香蕉:xiangjiao:xj 香蕉林:xiangjiaolin:xjl
马圈湾:majuanwan:mjw 马朝旭:mazhaoxu:mzx 马汝珩:maruheng:mrh 马道长:madaozhang:mdz 马都拉:madoula:mdl 驮子:duozi:dz 骂不还口:mabuhuankou:mbhk 骄矜:jiaojin:jj
验血:yanxie:yx 骠骑:piaoqi:pq 骨殖:gushi:gs 高丽参:gaolishen:gls 高句丽:gaogouli:ggl 高楼大厦:gaoloudasha:glds 高高地:gaogaode:ggd 鬼使神差:guishishenchai:gssc
鬼蜮伎俩:guiyujiliang:gyjl 魏学曾:weixueceng:wxc 鲁长老:luzhanglao:lzl 鲍长老:baozhanglao:bzl 鸡肋:jilei:jl 鸡血:jixie:jx 鸡血石:jixieshi:jxs 鹿死谁手:lusisheishou:lsss
黄柏:huangbo:hb 黄柏富:huangbofu:hbf 黄澄澄:huangdengdeng:hdd 黄疸:huangdan:hd 黄老邪:huanglaoye:hly 黄蕉风:huangjiaofeng:hjf 黄裳:huangchang:hc 黄陂:huangpi:hp
黄陂区:huangpiqu:hpq 黏着:nianzhuo:nz 黑颈鹤:heijinghe:hjh 默默地:momode:mmd 鼓乐:guyue:gy 鼓乐喧天:guyuexuantian:gyxt 鼓乐声:guyuesheng:gys 鼓乐齐鸣:guyueqiming:gyqm
鼻塞:bise:bs 鼻血:bixie:bx 龙门刨:longmenbao:lmb 龟兹:qiuci:qc 龟缩:guisuo:gs 龟裂:junlie:jl
";

        #endregion

        internal static string[] codes = new string[]{
"a     :阿啊吖嗄腌锕",
"ai    :爱埃碍矮挨唉哎哀皑癌蔼艾隘捱嗳嗌嫒瑷暧砹锿霭",
"an    :安按暗岸案俺氨胺鞍谙埯揞犴庵桉铵鹌黯",
"ang   :昂肮盎",
"ao    :凹奥敖熬翱袄傲懊澳坳拗嗷岙廒遨媪骜獒聱螯鏊鳌鏖",
"ba    :把八吧巴拔霸罢爸坝芭捌扒叭笆疤跋靶耙茇菝岜灞钯粑鲅魃",
"bai   :百白败摆柏佰拜稗捭掰",
"ban   :办半板班般版拌搬斑扳伴颁扮瓣绊阪坂钣瘢癍舨",
"bang  :帮棒邦榜梆膀绑磅蚌镑傍谤蒡浜",
"bao   :报保包剥薄胞暴宝饱抱爆堡苞褒雹豹鲍葆孢煲鸨褓趵龅",
"bei   :北被倍备背辈贝杯卑悲碑钡狈惫焙孛陂邶埤萆蓓呗悖碚鹎褙鐾鞴",
"ben   :本奔苯笨畚坌贲锛",
"beng  :泵崩绷甭蹦迸嘣甏",
"bi    :比必避闭辟笔壁臂毕彼逼币鼻蔽鄙碧蓖毙毖庇痹敝弊陛匕俾荜荸薜吡哔狴庳愎滗濞弼妣婢嬖璧畀铋秕裨筚箅篦舭襞跸髀",
"bian  :变边便编遍辩扁辨鞭贬卞辫匾弁苄忭汴缏飚煸砭碥窆褊蝙笾鳊",
"biao  :表标彪膘婊骠杓飑飙镖镳瘭裱鳔髟",
"bie   :别鳖憋瘪蹩",
"bin   :宾彬斌濒滨摈傧豳缤玢槟殡膑镔髌鬓",
"bing  :并病兵柄冰丙饼秉炳禀邴摒",
"bo    :波播伯拨博勃驳玻泊菠钵搏铂箔帛舶脖膊渤亳啵饽檗擘礴钹鹁簸跛踣",
"bu    :不部步布补捕卜哺埠簿怖卟逋瓿晡钚钸醭",
"ca    :擦嚓礤",
"cai   :采才材菜财裁彩猜睬踩蔡",
"can   :参残蚕灿餐惭惨孱骖璨粲黪",
"cang  :藏仓苍舱沧",
"cao   :草槽操糙曹嘈漕螬艚",
"ce    :测策侧册厕恻",
"cen   :岑涔",
"ceng  :层蹭",
"cha   :查差插察茶叉茬碴搽岔诧猹馇汊姹杈楂槎檫锸镲衩",
"chai  :柴拆豺侪钗瘥虿",
"chan  :产铲阐搀掺蝉馋谗缠颤冁谄蒇廛忏潺澶羼婵骣觇禅镡蟾躔",
"chang :长常场厂唱肠昌倡偿畅猖尝敞伥鬯苌菖徜怅惝阊娼嫦昶氅鲳",
"chao  :朝超潮巢抄钞嘲吵炒怊晁耖",
"che   :车彻撤扯掣澈坼砗",
"chen  :陈沉称衬尘臣晨郴辰忱趁伧谌谶抻嗔宸琛榇碜龀",
"cheng :成程称城承乘呈撑诚橙惩澄逞骋秤丞埕噌枨柽塍瞠铖铛裎蛏酲",
"chi   :持尺齿吃赤池迟翅斥耻痴匙弛驰侈炽傺坻墀茌叱哧啻嗤彳饬媸敕眵鸱瘛褫蚩螭笞篪豉踟魑",
"chong :虫充冲崇宠茺忡憧铳舂艟",
"chou  :抽仇臭酬畴踌稠愁筹绸瞅丑俦帱惆瘳雠",
"chu   :出处除初础触楚锄储橱厨躇雏滁矗搐亍刍怵憷绌杵楮樗褚蜍蹰黜",
"chuai :揣搋啜膪踹",
"chuan :传船穿串川椽喘舛遄巛氚钏舡",
"chuang:床创窗闯疮幢怆",
"chui  :吹垂锤炊捶陲棰槌",
"chun  :春纯醇椿唇淳蠢莼鹑蝽",
"chuo  :戳绰辍踔龊",
"ci    :此次刺磁雌词茨疵辞慈瓷赐茈呲祠鹚糍",
"cong  :从丛聪葱囱匆苁淙骢琮璁",
"cou   :凑楱辏腠",
"cu    :粗促醋簇蔟徂猝殂酢蹙蹴",
"cuan  :篡蹿窜汆撺爨镩",
"cui   :催脆淬粹摧崔瘁翠萃啐悴璀榱毳隹",
"cun   :存村寸忖皴",
"cuo   :错措撮磋搓挫厝嵯脞锉矬痤鹾蹉",
"da    :大打达答搭瘩耷哒嗒怛妲疸褡笪靼鞑",
"dai   :代带待袋戴呆歹傣殆贷逮怠埭甙呔岱迨骀绐玳黛",
"dan   :单但弹担蛋淡胆氮丹旦耽郸掸惮诞儋萏啖殚赕眈疸瘅聃箪",
"dang  :党当档挡荡谠凼菪宕砀裆",
"dao   :到道导刀倒稻岛捣盗蹈祷悼叨忉氘纛",
"de    :的得德锝",
"deng  :等灯登邓蹬瞪凳噔嶝戥磴镫簦",
"di    :地第低敌底帝抵滴弟递堤迪笛狄涤翟嫡蒂缔氐籴诋谛邸荻嘀娣绨柢棣觌砥碲睇镝羝骶",
"dia   :嗲",
"dian  :电点垫典店颠淀掂滇碘靛佃甸惦奠殿阽坫巅玷钿癜癫簟踮",
"diao  :调掉吊碉叼雕凋刁钓铞铫貂鲷",
"die   :迭跌爹碟蝶谍叠垤堞揲喋牒瓞耋蹀鲽",
"ding  :定顶钉丁订盯叮鼎锭仃啶玎腚碇町疔耵酊",
"diu   :丢铥",
"dong  :动东冬懂洞冻董栋侗恫垌咚岽峒氡胨胴硐鸫",
"dou   :斗豆兜抖陡逗痘蔸窦蚪篼",
"du    :度都毒独读渡杜堵镀督犊睹赌肚妒芏嘟渎椟牍蠹笃髑黩",
"duan  :断端段短锻缎椴煅簖",
"dui   :对队堆兑怼憝碓",
"dun   :盾吨顿蹲敦墩囤钝遁沌炖砘礅盹镦趸",
"duo   :多夺朵掇哆垛躲跺舵剁惰堕咄哚沲缍柁铎裰踱",
"e     :恶额俄蛾饿鹅讹娥厄扼遏鄂噩谔垩苊莪萼呃愕屙婀轭腭锇锷鹗颚鳄",
"ei    :诶",
"en    :恩蒽摁",
"er    :而二尔儿耳饵洱贰佴迩珥铒鸸鲕",
"fa    :发法阀乏伐罚筏珐垡砝",
"fan   :反翻范犯饭繁泛番凡烦返藩帆樊矾钒贩蕃蘩幡梵燔畈蹯",
"fang  :方放防访房纺仿妨芳肪坊邡枋钫舫鲂",
"fei   :非肥飞费废肺沸菲匪啡诽吠芾狒悱淝妃绯榧腓斐扉镄痱蜚篚翡霏鲱",
"fen   :分粉奋份粪纷芬愤酚吩氛坟焚汾忿偾瀵棼鲼鼢",
"feng  :风封蜂丰缝峰锋疯奉枫烽逢冯讽凤俸酆葑唪沣砜",
"fou   :否缶",
"fu    :复服副府夫负富附福伏符幅腐浮辅付腹妇孵覆扶辐傅佛缚父弗甫肤氟敷拂俘涪袱抚俯釜斧脯腑赴赋阜讣咐匐凫郛芙苻茯莩菔拊呋幞怫滏艴孚驸绂绋桴赙祓砩黻黼罘稃馥蚨蜉蝠蝮麸趺跗鲋鳆",
"ga    :噶嘎尬尕尜旮钆",
"gai   :改该盖概钙溉丐陔垓戤赅",
"gan   :干杆感敢赶甘肝秆柑竿赣坩苷尴擀泔淦澉绀橄旰矸疳酐",
"gang  :刚钢缸纲岗港杠冈肛戆罡筻",
"gao   :高搞告稿膏篙皋羔糕镐睾诰郜藁缟槔槁杲锆",
"ge    :个各革格割歌隔哥铬阁戈葛搁鸽胳疙蛤鬲仡哿圪塥嗝搿膈硌镉袼虼舸骼",
"gen   :根跟亘茛哏艮",
"geng  :更耕颈庚羹埂耿梗哽赓绠鲠",
"gong  :工公共供功攻巩贡汞宫恭龚躬弓拱珙肱蚣觥",
"gou   :够构沟狗钩勾购苟垢佝诟岣遘媾缑枸觏彀笱篝鞲",
"gu    :鼓固古骨故顾股谷估雇孤姑辜菇咕箍沽蛊嘏诂菰崮汩梏轱牯牿臌毂瞽罟钴锢鸪痼蛄酤觚鲴鹘",
"gua   :挂刮瓜剐寡褂卦诖呱栝胍鸹",
"guai  :怪乖拐",
"guan  :关管观官灌贯惯冠馆罐棺倌莞掼涫盥鹳矜鳏",
"guang :光广逛咣犷桄胱",
"gui   :规贵归硅鬼轨龟桂瑰圭闺诡癸柜跪刽匦刿庋宄妫桧炅晷皈簋鲑鳜",
"gun   :滚辊棍衮绲磙鲧",
"guo   :国过果锅郭裹馘埚掴呙帼崞猓椁虢聒蜾蝈",
"ha    :哈铪",
"hai   :还海害孩骸氦亥骇嗨胲醢",
"han   :含焊旱喊汉寒汗函韩酣憨邯涵罕翰撼捍憾悍邗菡撖阚瀚晗焓顸颔蚶鼾",
"hang  :航夯杭沆绗珩颃",
"hao   :好号毫耗豪郝浩壕嚎蒿薅嗥嚆濠灏昊皓颢蚝",
"he    :和合河何核赫荷褐喝贺呵禾盒菏貉阂涸鹤诃劾壑嗬阖纥曷盍颌蚵翮",
"hei   :黑嘿",
"hen   :很狠痕恨",
"heng  :横衡恒哼亨蘅桁",
"hong  :红洪轰烘哄虹鸿宏弘黉訇讧荭蕻薨闳泓",
"hou   :后候厚侯喉猴吼堠後逅瘊篌糇鲎骺",
"hu    :护互湖呼户弧乎胡糊虎忽瑚壶葫蝴狐唬沪冱唿囫岵猢怙惚浒滹琥槲轷觳烀煳戽扈祜瓠鹄鹕鹱笏醐斛",
"hua   :化花话划滑华画哗猾骅桦砉铧",
"huai  :坏怀淮槐徊踝",
"huan  :环换欢缓患幻焕桓唤痪豢涣宦郇奂萑擐圜獾洹浣漶寰逭缳锾鲩鬟",
"huang :黄簧荒皇慌蝗磺凰惶煌晃幌恍谎隍徨湟潢遑璜肓癀蟥篁鳇",
"hui   :会回灰挥辉汇毁慧恢绘惠徽蛔悔卉晦贿秽烩讳诲诙茴荟蕙咴哕喙隳洄浍彗缋珲晖恚虺蟪麾",
"hun   :混浑荤昏婚魂诨馄阍溷",
"huo   :活或火货获伙霍豁惑祸劐藿攉嚯夥钬锪镬耠蠖",
"ji    :级及机极几积给基记己计集即际季激济技击继急剂既纪寄挤鸡迹绩吉脊辑籍疾肌棘畸圾稽箕饥讥姬缉汲嫉蓟冀伎祭悸寂忌妓藉丌亟乩剞佶偈诘墼芨芰荠蒺蕺掎叽咭哜唧岌嵴洎屐骥畿玑楫殛戟戢赍觊犄齑矶羁嵇稷瘠虮笈笄暨跻跽霁鲚鲫髻麂",
"jia   :加家架价甲夹假钾贾稼驾嘉枷佳荚颊嫁伽郏葭岬浃迦珈戛胛恝铗镓痂瘕袷蛱笳袈跏",
"jian  :间件见建坚减检践尖简碱剪艰渐肩键健柬鉴剑歼监兼奸箭茧舰俭笺煎缄硷拣捡荐槛贱饯溅涧僭谏谫菅蒹搛湔蹇謇缣枧楗戋戬牮犍毽腱睑锏鹣裥笕翦趼踺鲣鞯",
"jiang :将降讲江浆蒋奖疆僵姜桨匠酱茳洚绛缰犟礓耩糨豇",
"jiao  :较教交角叫脚胶浇焦搅酵郊铰窖椒礁骄娇嚼矫侥狡饺缴绞剿轿佼僬艽茭挢噍峤徼姣敫皎鹪蛟醮跤鲛",
"jie   :结阶解接节界截介借届街揭洁杰竭皆秸劫桔捷睫姐戒藉芥疥诫讦拮喈嗟婕孑桀碣疖颉蚧羯鲒骱",
"jin   :进金近紧斤今尽仅劲浸禁津筋锦晋巾襟谨靳烬卺荩堇噤馑廑妗缙瑾槿赆觐衿",
"jing  :经精京径井静竟晶净境镜景警茎敬惊睛竞荆兢鲸粳痉靖刭儆阱菁獍憬泾迳弪婧肼胫腈旌",
"jiong :炯窘迥扃",
"jiu   :就九旧究久救酒纠揪玖韭灸厩臼舅咎疚僦啾阄柩桕鸠鹫赳鬏",
"ju    :具据局举句聚距巨居锯剧矩拒鞠拘狙疽驹菊咀沮踞俱惧炬倨讵苣苴莒掬遽屦琚椐榘榉橘犋飓钜锔窭裾趄醵踽龃雎鞫",
"juan  :卷捐鹃娟倦眷绢鄄狷涓桊蠲锩镌隽",
"jue   :决觉绝掘撅攫抉倔爵诀厥劂谲矍蕨噘噱崛獗孓珏桷橛爝镢蹶觖",
"jun   :军均菌君钧峻俊竣浚郡骏捃皲筠麇",
"ka    :卡喀咖咯佧咔胩",
"kai   :开凯揩楷慨剀垲蒈忾恺铠锎锴",
"kan   :看刊坎堪勘砍侃莰戡龛瞰",
"kang  :抗康炕慷糠扛亢伉闶钪",
"kao   :考靠拷烤尻栲犒铐",
"ke    :可克科刻客壳颗棵柯坷苛磕咳渴课嗑岢恪溘骒缂珂轲氪瞌钶锞稞疴窠颏蝌髁",
"ken   :肯啃垦恳裉",
"keng  :坑吭铿",
"kong  :孔空控恐倥崆箜",
"kou   :口扣抠寇芤蔻叩眍筘",
"ku    :苦库枯酷哭窟裤刳堀喾绔骷",
"kua   :跨夸垮挎胯侉",
"kuai  :快块筷侩蒯郐哙狯脍",
"kuan  :宽款髋",
"kuang :况矿狂框匡筐眶旷诓诳邝圹夼哐纩贶",
"kui   :奎溃馈亏盔岿窥葵魁傀愧馗匮夔隗蒉揆喹喟悝愦逵暌睽聩蝰篑跬",
"kun   :困昆坤捆悃阃琨锟醌鲲髡",
"kuo   :扩括阔廓蛞",
"la    :拉啦蜡腊垃喇辣剌邋旯砬瘌",
"lai   :来赖莱崃徕涞濑赉睐铼癞籁",
"lan   :兰烂蓝览栏婪拦篮阑澜谰揽懒缆滥岚漤榄斓罱镧褴",
"lang  :浪朗郎狼琅榔廊莨蒗啷阆锒稂螂",
"lao   :老劳牢涝捞佬姥酪烙唠崂栳铑铹痨耢醪",
"le    :了乐勒肋仂叻泐鳓",
"lei   :类雷累垒泪镭蕾磊儡擂肋羸诔嘞嫘缧檑耒酹",
"leng  :冷棱楞塄愣",
"li    :理里利力立离例历粒厘礼李隶黎璃励犁梨丽厉篱狸漓鲤莉荔吏栗砾傈俐痢沥哩俪俚郦坜苈莅蓠藜呖唳喱猁溧澧逦娌嫠骊缡枥栎轹戾砺詈罹锂鹂疠疬蛎蜊蠡笠篥粝醴跞雳鲡鳢黧",
"lia   :俩",
"lian  :连联练炼脸链莲镰廉怜涟帘敛恋蔹奁潋濂琏楝殓臁裢裣蠊鲢",
"liang :量两粮良亮梁凉辆粱晾谅墚椋踉靓魉",
"liao  :料疗辽僚撩聊燎寥潦撂镣廖蓼尥嘹獠寮缭钌鹩",
"lie   :列裂烈劣猎冽埒捩咧洌趔躐鬣",
"lin   :林磷临邻淋麟琳霖鳞凛赁吝蔺啉嶙廪懔遴檩辚膦瞵粼躏",
"ling  :领另零令灵岭铃龄凌陵拎玲菱伶羚酃苓呤囹泠绫柃棂瓴聆蛉翎鲮",
"liu   :流六留刘硫柳馏瘤溜琉榴浏遛骝绺旒熘锍镏鹨鎏",
"long  :龙垄笼隆聋咙窿拢陇垅茏泷珑栊胧砻癃",
"lou   :漏楼娄搂篓陋偻蒌喽嵝镂瘘耧蝼髅",
"lu    :路率露绿炉律虑滤陆氯鲁铝录旅卢吕芦颅庐掳卤虏麓碌赂鹿潞禄戮驴侣履屡缕垆撸噜闾泸渌漉逯璐栌榈橹轳辂辘氇胪膂镥稆鸬鹭褛簏舻鲈",
"luan  :卵乱峦挛孪滦脔娈栾鸾銮",
"lue   :略掠锊",
"lun   :论轮伦抡仑沦纶囵",
"luo   :落罗螺洛络逻萝锣箩骡裸骆倮蠃荦捋摞猡泺漯珞椤脶镙瘰雒",
"m     :呒",
"ma    :马麻吗妈骂嘛码玛蚂唛犸嬷杩蟆",
"mai   :麦脉卖买埋迈劢荬霾",
"man   :满慢曼漫蔓瞒馒蛮谩墁幔缦熳镘颟螨鳗鞔",
"mang  :忙芒盲茫氓莽邙漭硭蟒",
"mao   :毛矛冒貌贸帽猫茅锚铆卯茂袤茆峁泖瑁昴牦耄旄懋瞀蟊髦",
"me    :么麽",
"mei   :没每美煤霉酶梅妹眉玫枚媒镁昧寐媚莓嵋猸浼湄楣镅鹛袂魅",
"men   :们门闷扪焖懑钔",
"meng  :孟猛蒙盟梦萌锰檬勐甍瞢懵朦礞虻蜢蠓艋艨",
"mi    :米密迷蜜秘眯醚靡糜谜弥觅泌幂芈谧蘼咪嘧猕汨宓弭脒祢敉糸縻麋",
"mian  :面棉免绵眠冕勉娩缅沔渑湎腼眄",
"miao  :苗秒描庙妙瞄藐渺喵邈缈缪杪淼眇鹋",
"mie   :灭蔑咩蠛篾",
"min   :民敏抿皿悯闽苠岷闵泯缗玟珉愍黾鳘",
"ming  :命明名鸣螟铭冥茗溟暝瞑酩",
"miu   :谬",
"mo    :磨末模膜摸墨摩莫抹默摹蘑魔沫漠寞陌谟茉蓦馍嫫殁镆秣瘼耱貊貘",
"mou   :某谋牟侔哞眸蛑蝥鍪",
"mu    :亩目木母墓幕牧姆穆拇牡暮募慕睦仫坶苜沐毪钼",
"n     :嗯",
"na    :那哪拿纳钠呐娜捺肭镎衲",
"nai   :耐奶乃氖奈鼐艿萘柰",
"nan   :南难男喃囝囡楠腩蝻赧",
"nang  :囊攮囔馕曩",
"nao   :脑闹挠恼淖孬垴呶猱瑙硇铙蛲",
"ne    :呢讷",
"nei   :内馁",
"nen   :嫩恁",
"neng  :能",
"ni    :你泥尼逆拟妮霓倪匿腻溺伲坭猊怩昵旎慝睨铌鲵",
"nian  :年念粘蔫拈碾撵捻酿廿埝辇黏鲇鲶",
"niang :娘",
"niao  :尿鸟茑嬲脲袅",
"nie   :镍啮涅捏聂孽镊乜陧蘖嗫颞臬蹑",
"nin   :您",
"ning  :宁凝拧柠狞泞佞苎咛甯聍",
"niu   :牛扭钮纽狃忸妞",
"nong  :农弄浓脓侬哝",
"nou   :耨",
"nu    :女奴努怒弩胬孥驽恧钕衄",
"nuan  :暖",
"nue   :虐",
"nuo   :诺挪懦糯傩搦喏锘",
"o     :哦噢",
"ou    :欧偶鸥殴藕呕沤讴怄瓯耦",
"pa    :怕爬帕啪趴琶葩杷筢",
"pai   :派排拍牌哌徘湃俳蒎",
"pan   :判盘叛潘攀磐盼畔胖爿泮袢襻蟠蹒",
"pang  :旁乓庞耪胖彷滂逄螃",
"pao   :跑炮刨抛泡咆袍匏狍庖脬疱",
"pei   :配培陪胚呸裴赔佩沛辔帔旆锫醅霈",
"pen   :喷盆湓",
"peng  :碰棚蓬朋捧膨砰抨烹澎彭硼篷鹏堋嘭怦蟛",
"pi    :批皮坯脾疲砒霹披劈琵毗啤匹痞僻屁譬丕仳陴邳郫圮鼙芘擗噼庀淠媲纰枇甓睥罴铍癖疋蚍蜱貔",
"pian  :片偏篇骗谝骈犏胼翩蹁",
"piao  :票漂飘瓢剽嘌嫖缥殍瞟螵",
"pie   :撇瞥丿苤氕",
"pin   :品贫频拼聘拚姘嫔榀牝颦",
"ping  :平评瓶凭苹乒坪萍屏俜娉枰鲆",
"po    :破迫坡泼颇婆魄粕叵鄱珀攴钋钷皤笸",
"pou   :剖裒掊",
"pu    :普谱扑埔铺葡朴蒲仆莆菩圃浦曝瀑匍噗溥濮璞氆镤镨蹼",
"qi    :起其气期七器齐奇汽企漆欺旗畦启弃歧栖戚妻凄柒沏棋崎脐祈祁骑岂乞契砌迄泣讫亓俟圻芑芪萁萋葺蕲嘁屺岐汔淇骐绮琪琦杞桤槭耆欹祺憩碛颀蛴蜞綦綮蹊鳍麒",
"qia   :恰掐洽葜髂",
"qian  :前千钱浅签迁铅潜牵钳谴扦钎仟谦乾黔遣堑嵌欠歉倩佥阡芊芡茜荨掮岍悭慊骞搴褰缱椠肷愆钤虔箬箝",
"qiang :强枪抢墙腔呛羌蔷戕嫱樯戗炝锖锵镪襁蜣羟跄",
"qiao  :桥瞧巧敲乔蕉橇锹悄侨鞘撬翘峭俏窍劁诮谯荞愀憔樵硗跷鞒",
"qie   :切且茄怯窃郄惬妾挈锲箧",
"qin   :亲侵勤秦钦琴芹擒禽寝沁芩揿吣嗪噙溱檎锓覃螓衾",
"qing  :情清青轻倾请庆氢晴卿擎氰顷苘圊檠磬蜻罄箐謦鲭黥",
"qiong :穷琼邛茕穹蛩筇跫銎",
"qiu   :求球秋丘邱囚酋泅俅巯犰湫逑遒楸赇虬蚯蝤裘糗鳅鼽",
"qu    :去区取曲渠屈趋驱趣蛆躯娶龋诎劬蕖蘧岖衢阒璩觑氍朐祛磲鸲癯蛐蠼麴瞿黢",
"quan  :全权圈劝泉醛颧痊拳犬券诠荃悛绻辁畎铨蜷筌鬈",
"que   :确却缺炔瘸鹊榷雀阕阙悫",
"qun   :群裙逡",
"ran   :然燃染冉苒蚺髯",
"rang  :让壤嚷瓤攘禳穰",
"rao   :绕扰饶荛娆桡",
"re    :热惹",
"ren   :人认任仁刃忍壬韧妊纫仞荏葚饪轫稔衽",
"reng  :仍扔",
"ri    :日",
"rong  :容溶荣熔融绒戎茸蓉冗嵘狨榕肜蝾",
"rou   :肉揉柔糅蹂鞣",
"ru    :如入儒乳茹蠕孺辱汝褥蓐薷嚅洳溽濡缛铷襦颥",
"ruan  :软阮朊",
"rui   :瑞锐蕊芮蕤枘睿蚋",
"run   :润闰",
"ruo   :弱若偌",
"sa    :撒萨洒卅仨挲脎飒",
"sai   :塞赛腮鳃噻",
"san   :三散叁伞馓毵糁",
"sang  :桑丧嗓搡磉颡",
"sao   :扫搔骚嫂埽缫缲臊瘙鳋",
"se    :色瑟涩啬铯穑",
"sen   :森",
"seng  :僧",
"sha   :沙杀砂啥纱莎刹傻煞唼歃铩痧裟霎鲨",
"shai  :筛晒",
"shan  :山闪善珊扇陕苫杉删煽衫擅赡膳汕缮剡讪鄯埏芟潸姗嬗骟膻钐疝蟮舢跚鳝",
"shang :上商伤尚墒赏晌裳垧绱殇熵觞",
"shao  :少烧稍绍哨梢捎芍勺韶邵劭苕潲蛸筲艄",
"she   :社设射摄舌涉舍蛇奢赊赦慑厍佘猞滠歙畲麝",
"shen  :深身神伸甚渗沈肾审申慎砷呻娠绅婶诜谂莘哂渖椹胂矧蜃",
"sheng :生胜声省升盛绳剩圣牲甥嵊晟眚笙",
"shi   :是时十使事实式识世试石什示市史师始施士势湿适食失视室氏蚀诗释拾饰驶狮尸虱矢屎柿拭誓逝嗜噬仕侍恃谥埘莳蓍弑轼贳炻铈螫舐筮酾豕鲥鲺",
"shou  :手受收首守授寿兽售瘦狩绶艏",
"shu   :数书树属术输述熟束鼠疏殊舒蔬薯叔署枢梳抒淑赎孰暑曙蜀黍戍竖墅庶漱恕丨倏塾菽摅沭澍姝纾毹腧殳秫",
"shua  :刷耍唰",
"shuai :衰帅摔甩蟀",
"shuan :栓拴闩涮",
"shuang:双霜爽孀",
"shui  :水谁睡税",
"shun  :顺吮瞬舜",
"shuo  :说硕朔烁蒴搠妁槊铄",
"si    :四思死斯丝似司饲私撕嘶肆寺嗣伺巳厮兕厶咝汜泗澌姒驷缌祀锶鸶耜蛳笥",
"song  :松送宋颂耸怂讼诵凇菘崧嵩忪悚淞竦",
"sou   :搜艘擞嗽叟薮嗖嗾馊溲飕瞍锼螋",
"su    :素速苏塑缩俗诉宿肃酥粟僳溯夙谡蔌嗉愫涑簌觫稣",
"suan  :算酸蒜狻",
"sui   :随穗碎虽岁隋绥髓遂隧祟谇荽濉邃燧眭睢",
"sun   :损孙笋荪狲飧榫隼",
"suo   :所缩锁索蓑梭唆琐唢嗦嗍娑桫睃羧",
"ta    :他它她塔踏塌獭挞蹋闼溻遢榻沓铊趿鳎",
"tai   :台太态胎抬泰苔酞汰邰薹肽炱钛跆鲐",
"tan   :谈碳探炭坦贪滩坍摊瘫坛檀痰潭谭毯袒叹郯澹昙忐钽锬",
"tang  :堂糖唐塘汤搪棠膛倘躺淌趟烫傥帑溏瑭樘铴镗耥螗螳羰醣",
"tao   :套讨逃陶萄桃掏涛滔绦淘鼗啕洮韬焘饕",
"te    :特忒忑铽",
"teng  :腾疼藤誊滕",
"ti    :提题体替梯惕剔踢锑蹄啼嚏涕剃屉倜悌逖缇鹈裼醍",
"tian  :天田添填甜恬舔腆掭忝阗殄畋",
"tiao  :条跳挑迢眺佻祧窕蜩笤粜龆鲦髫",
"tie   :铁贴帖萜餮",
"ting  :听停庭挺廷厅烃汀亭艇莛葶婷梃铤蜓霆",
"tong  :同通统铜痛筒童桶桐酮瞳彤捅佟仝茼嗵恸潼砼",
"tou   :头投透偷钭骰",
"tu    :图土突途徒凸涂吐兔屠秃堍荼菟钍酴",
"tuan  :团湍抟彖疃",
"tui   :推退腿颓蜕褪煺",
"tun   :吞屯臀氽饨暾豚",
"tuo   :脱拖托妥椭鸵陀驮驼拓唾乇佗坨庹沱柝橐砣箨酡跎鼍",
"wa    :瓦挖哇蛙洼娃袜佤娲腽",
"wai   :外歪",
"wan   :完万晚弯碗顽湾挽玩豌丸烷皖惋宛婉腕剜芄菀纨绾琬脘畹蜿",
"wang  :往王望网忘妄亡旺汪枉罔尢惘辋魍",
"wei   :为位委围维唯卫微伟未威危尾谓喂味胃魏伪违韦畏纬巍桅惟潍苇萎蔚渭尉慰偎诿隈葳薇囗帏帷崴嵬猥猬闱沩洧涠逶娓玮韪軎炜煨痿艉鲔",
"wen   :问温文稳纹闻蚊瘟吻紊刎阌汶璺雯",
"weng  :嗡翁瓮蓊蕹",
"wo    :我握窝蜗涡沃挝卧斡倭莴喔幄渥肟硪龌",
"wu    :无五物武务误伍舞污悟雾午屋乌吴诬钨巫呜芜梧吾毋捂侮坞戊晤勿兀仵阢邬圬芴唔庑怃忤浯寤迕妩婺骛杌牾焐鹉鹜痦蜈鋈鼯",
"xi    :系席西习细吸析喜洗铣稀戏隙希息袭锡烯牺悉惜溪昔熙硒矽晰嘻膝夕熄汐犀檄媳僖兮隰郗菥葸蓰奚唏徙饩阋浠淅屣嬉玺樨曦觋欷熹禊禧皙穸蜥螅蟋舄舾羲粞翕醯鼷",
"xia   :下夏吓狭霞瞎虾匣辖暇峡侠厦呷狎遐瑕柙硖罅黠",
"xian  :线现先县限显鲜献险陷宪纤掀弦腺锨仙咸贤衔舷闲涎嫌馅羡冼苋莶藓岘猃暹娴氙燹祆鹇痫蚬筅籼酰跣跹霰",
"xiang :想向相象响项箱乡香像详橡享湘厢镶襄翔祥巷芗葙饷庠骧缃蟓鲞飨",
"xiao  :小消削效笑校销硝萧肖孝霄哮嚣宵淆晓啸哓崤潇逍骁绡枭枵筱箫魈",
"xie   :些写斜谢协械卸屑鞋歇邪胁蟹泄泻楔蝎挟携谐懈偕亵勰燮薤撷獬廨渫瀣邂绁缬榭榍躞",
"xin   :新心信锌芯辛欣薪忻衅囟馨昕歆鑫",
"xing  :行性形型星兴醒姓幸腥猩惺刑邢杏陉荇荥擤饧悻硎",
"xiong :雄胸兄凶熊匈汹芎",
"xiu   :修锈休袖秀朽羞嗅绣咻岫馐庥溴鸺貅髹",
"xu    :续许须需序虚絮畜叙蓄绪徐墟戌嘘酗旭恤婿诩勖圩蓿洫溆顼栩煦盱胥糈醑",
"xuan  :选旋宣悬玄轩喧癣眩绚儇谖萱揎泫渲漩璇楦暄炫煊碹铉镟痃",
"xue   :学血雪穴靴薛谑泶踅鳕",
"xun   :训旬迅讯寻循巡勋熏询驯殉汛逊巽埙荀蕈薰峋徇獯恂洵浔曛醺鲟",
"ya    :压亚呀牙芽雅蚜鸭押鸦丫崖衙涯哑讶伢垭揠岈迓娅琊桠氩砑睚痖",
"yan   :验研严眼言盐演岩沿烟延掩宴炎颜燕衍焉咽阉淹蜒阎奄艳堰厌砚雁唁彦焰谚厣赝俨偃兖谳郾鄢菸崦恹闫阏湮滟妍嫣琰檐晏胭焱罨筵酽魇餍鼹",
"yang  :样养氧扬洋阳羊秧央杨仰殃鸯佯疡痒漾徉怏泱炀烊恙蛘鞅",
"yao   :要药摇腰咬邀耀疟妖瑶尧遥窑谣姚舀夭爻吆崾徭幺珧杳轺曜肴鹞窈繇鳐",
"ye    :也业页叶液夜野爷冶椰噎耶掖曳腋靥谒邺揶晔烨铘",
"yi    :一以义意已移医议依易乙艺益异宜仪亿遗伊役衣疑亦谊翼译抑忆疫壹揖铱颐夷胰沂姨彝椅蚁倚矣邑屹臆逸肄裔毅溢诣翌绎刈劓佚佾诒圯埸懿苡荑薏弈奕挹弋呓咦咿噫峄嶷猗饴怿怡悒漪迤驿缢殪轶贻旖熠眙钇镒镱痍瘗癔翊蜴舣羿翳酏黟",
"yin   :因引阴印音银隐饮荫茵殷姻吟淫寅尹胤鄞垠堙茚吲喑狺夤洇氤铟瘾窨蚓霪龈",
"ying  :应影硬营英映迎樱婴鹰缨莹萤荧蝇赢盈颖嬴郢茔莺萦蓥撄嘤膺滢潆瀛瑛璎楹媵鹦瘿颍罂",
"yo    :哟唷",
"yong  :用勇永拥涌蛹庸佣臃痈雍踊咏泳恿俑壅墉喁慵邕镛甬鳙饔",
"you   :有由又油右友优幼游尤诱犹幽悠忧邮铀酉佑釉卣攸侑莠莜莸呦囿宥柚猷牖铕疣蚰蚴蝣鱿黝鼬",
"yu    :于与育鱼雨玉余遇预域语愈渔予羽愚御欲宇迂淤盂榆虞舆俞逾愉渝隅娱屿禹芋郁吁喻峪狱誉浴寓裕豫驭禺毓伛俣谀谕萸蓣揄圄圉嵛狳饫馀庾阈鬻妪妤纡瑜昱觎腴欤於煜熨燠聿钰鹆鹬瘐瘀窬窳蜮蝓竽臾舁雩龉",
"yuan  :员原圆源元远愿院缘援园怨鸳渊冤垣袁辕猿苑垸塬芫掾沅媛瑗橼爰眢鸢螈箢鼋",
"yue   :月越约跃曰阅钥岳粤悦龠瀹樾刖钺",
"yun   :运云匀允孕耘郧陨蕴酝晕韵郓芸狁恽愠纭韫殒昀氲熨",
"za    :杂咱匝砸咋咂",
"zai   :在再载栽灾哉宰崽甾",
"zan   :赞咱暂攒拶瓒昝簪糌趱錾",
"zang  :脏葬赃奘驵臧",
"zao   :造早遭燥凿糟枣皂藻澡蚤躁噪灶唣",
"ze    :则择责泽仄赜啧帻迮昃笮箦舴",
"zei   :贼",
"zen   :怎谮",
"zeng  :增曾憎赠缯甑罾锃",
"zha   :扎炸闸铡轧渣喳札眨栅榨乍诈揸吒咤哳砟痄蚱齄",
"zhai  :寨摘窄斋宅债砦瘵",
"zhan  :战展站占瞻毡詹沾盏斩辗崭蘸栈湛绽谵搌旃",
"zhang :张章掌仗障胀涨账樟彰漳杖丈帐瘴仉鄣幛嶂獐嫜璋蟑",
"zhao  :照找招召赵爪罩沼兆昭肇诏棹钊笊",
"zhe   :这着者折哲浙遮蛰辙锗蔗谪摺柘辄磔鹧褶蜇赭",
"zhen  :真针阵镇振震珍诊斟甄砧臻贞侦枕疹圳蓁浈缜桢榛轸赈胗朕祯畛稹鸩箴",
"zheng :争正政整证征蒸症郑挣睁狰怔拯帧诤峥徵钲铮筝",
"zhi   :之制治只质指直支织止至置志值知执职植纸致枝殖脂智肢秩址滞汁芝吱蜘侄趾旨挚掷帜峙稚炙痔窒卮陟郅埴芷摭帙忮彘咫骘栉枳栀桎轵轾贽胝膣祉祗黹雉鸷痣蛭絷酯跖踬踯豸觯",
"zhong :中种重众钟终忠肿仲盅衷冢锺螽舯踵",
"zhou  :轴周洲州皱骤舟诌粥肘帚咒宙昼荮啁妯纣绉胄碡籀酎",
"zhu   :主注著住助猪铸株筑柱驻逐祝竹贮珠朱诸蛛诛烛煮拄瞩嘱蛀伫侏邾茱洙渚潴杼槠橥炷铢疰瘃竺箸舳翥躅麈",
"zhua  :抓",
"zhuai :拽",
"zhuan :转专砖撰赚篆啭馔颛",
"zhuang:装状壮庄撞桩妆僮",
"zhui  :追锥椎赘坠缀惴骓缒",
"zhun  :准谆肫窀",
"zhuo  :捉桌拙卓琢茁酌啄灼浊倬诼擢浞涿濯焯禚斫镯",
"zi    :子自资字紫仔籽姿兹咨滋淄孜滓渍谘嵫姊孳缁梓辎赀恣眦锱秭耔笫粢趑觜訾龇鲻髭",
"zong  :总纵宗综棕鬃踪偬枞腙粽",
"zou   :走邹奏揍诹陬鄹驺鲰",
"zu    :组族足阻祖租卒诅俎菹镞",
"zuan  :钻纂攥缵躜",
"zui   :最罪嘴醉蕞",
"zun   :尊遵撙樽鳟",
"zuo   :作做左座坐昨佐柞阼唑嘬怍胙祚"};

        internal static short[][] hashes = new short[][] {
new short[]{23, 70, 96, 128, 154, 165, 172, 195},
new short[]{25, 35, 87, 108, 120, 128, 132, 137, 168, 180, 325, 334, 336, 353, 361, 380},
new short[]{23, 34, 46, 81, 82, 87, 134, 237, 255, 288, 317, 322, 354, 359},
new short[]{7, 11, 37, 49, 53, 56, 131, 132, 146, 176, 315, 372},
new short[]{11, 69, 73, 87, 96, 103, 159, 175, 195, 296, 298, 359, 361},
new short[]{57, 87, 115, 126, 149, 244, 282, 298, 308, 345, 355},
new short[]{19, 37, 117, 118, 141, 154, 196, 216, 267, 301, 327, 333, 337, 347},
new short[]{4, 11, 59, 61, 62, 87, 119, 169, 183, 198, 262, 334, 362, 380},
new short[]{37, 135, 167, 170, 246, 250, 334, 341, 351, 354, 386, 390, 398},
new short[]{5, 6, 52, 55, 76, 146, 165, 244, 256, 266, 300, 318, 331},
new short[]{6, 71, 94, 129, 137, 141, 169, 179, 225, 226, 235, 248, 289, 290, 333, 345, 391},
new short[]{0, 33, 37, 62, 90, 131, 205, 246, 268, 343, 349, 380},
new short[]{31, 62, 85, 115, 117, 150, 159, 167, 171, 204, 215, 252, 343},
new short[]{69, 81, 98, 140, 165, 195, 239, 240, 259, 265, 329, 368, 375, 392, 393},
new short[]{13, 81, 82, 123, 132, 144, 154, 165, 334, 336, 345, 348, 349, 355, 367, 377, 383},
new short[]{31, 32, 44, 57, 76, 83, 87, 129, 151, 172, 176, 183, 184, 193, 221, 235, 285, 288, 305},
new short[]{10, 14, 60, 76, 85, 97, 115, 125, 128, 130, 286, 288, 301, 313, 382},
new short[]{62, 128, 136, 175, 211, 240, 254, 273, 274, 317, 330, 344, 349, 360, 380},
new short[]{29, 47, 52, 116, 126, 127, 130, 133, 191, 284, 288, 306, 353, 361, 383},
new short[]{1, 15, 25, 67, 83, 90, 117, 121, 150, 228, 308, 324, 336, 351, 386},
new short[]{34, 37, 67, 101, 103, 117, 127, 165, 168, 254, 267, 272, 274, 288, 305, 310, 323, 329, 333, 358, 378},
new short[]{5, 74, 103, 135, 163, 165, 171, 244, 262, 266, 334, 352, 390, 397},
new short[]{4, 17, 95, 125, 165, 186, 203, 221, 252, 282, 317, 333, 339, 348, 351, 353},
new short[]{74, 79, 81, 84, 92, 110, 116, 117, 131, 132, 154, 199, 241, 251, 300, 306, 349, 359, 383, 387},
new short[]{40, 83, 127, 144, 161, 188, 249, 288, 344, 382, 388},
new short[]{8, 55, 61, 76, 85, 98, 111, 127, 186, 230, 241, 247, 267, 287, 327, 341, 344, 347, 359, 364},
new short[]{20, 59, 69, 80, 117, 129, 176, 186, 191, 237, 275, 289, 309, 338, 375, 380},
new short[]{5, 15, 25, 35, 40, 129, 174, 236, 274, 337, 347},
new short[]{14, 22, 47, 56, 87, 120, 129, 144, 155, 160, 237, 283, 284, 309, 327, 337, 365, 372},
new short[]{1, 14, 47, 132, 198, 254, 255, 300, 310, 335, 336, 372},
new short[]{2, 36, 64, 96, 125, 176, 184, 190, 211, 271, 308, 315, 367},
new short[]{20, 76, 79, 81, 110, 117, 120, 129, 182, 192, 235, 353, 378},
new short[]{37, 83, 88, 92, 111, 127, 243, 303, 324, 325, 348, 353, 359, 371, 377},
new short[]{5, 87, 90, 124, 127, 180, 259, 288, 290, 302, 312, 313, 324, 332},
new short[]{55, 62, 89, 98, 108, 132, 168, 240, 248, 322, 325, 327, 347, 353, 391, 396},
new short[]{4, 8, 13, 35, 37, 39, 41, 64, 111, 174, 212, 245, 248, 251, 263, 288, 335, 373, 375},
new short[]{10, 39, 93, 110, 168, 227, 228, 254, 288, 336, 378, 381},
new short[]{75, 92, 122, 176, 198, 211, 214, 283, 334, 353, 359, 379, 386},
new short[]{5, 8, 13, 19, 57, 87, 104, 125, 130, 176, 202, 249, 252, 290, 309, 391},
new short[]{88, 132, 173, 176, 235, 247, 253, 292, 324, 328, 339, 359},
new short[]{19, 32, 61, 84, 87, 118, 120, 125, 129, 132, 181, 190, 288, 290, 331, 355, 359, 366},
new short[]{13, 25, 46, 126, 140, 157, 165, 225, 226, 252, 288, 304, 327, 353, 378},
new short[]{12, 14, 26, 56, 72, 95, 131, 132, 134, 142, 253, 298, 337, 361, 391},
new short[]{4, 18, 37, 49, 87, 93, 196, 225, 226, 246, 248, 250, 255, 310, 354, 358},
new short[]{64, 87, 110, 111, 128, 135, 151, 165, 177, 188, 191, 268, 312, 334, 352, 354, 357, 371},
new short[]{10, 17, 19, 30, 40, 48, 81, 97, 125, 129, 130, 182, 234, 305, 328, 393},
new short[]{13, 69, 80, 114, 192, 200, 235, 343, 345, 353, 354, 360, 374, 378, 383},
new short[]{83, 87, 94, 105, 107, 124, 144, 153, 219, 290, 298, 324, 349, 358, 367},
new short[]{10, 36, 142, 169, 221, 232, 241, 246, 346, 347, 375, 383, 390},
new short[]{26, 104, 126, 143, 176, 186, 241, 247, 250, 318, 320, 333, 360},
new short[]{66, 92, 116, 148, 191, 215, 254, 333, 334, 335, 336, 351, 353, 358, 380},
new short[]{9, 37, 55, 56, 76, 79, 90, 111, 122, 124, 161, 192, 247, 313, 353, 359, 374},
new short[]{17, 30, 34, 56, 64, 68, 90, 125, 151, 168, 176, 188, 286, 333, 338, 360},
new short[]{26, 143, 173, 182, 190, 194, 246, 284, 286, 328, 333, 355, 357, 360, 362, 363, 377, 380},
new short[]{1, 13, 87, 122, 168, 171, 186, 201, 297, 328, 349, 352, 380},
new short[]{18, 39, 61, 88, 98, 123, 129, 131, 148, 162, 165, 243, 285, 314, 340, 349, 360, 377, 378},
new short[]{67, 98, 117, 118, 122, 128, 156, 174, 184, 207, 244, 250, 330, 335, 342, 372, 375},
new short[]{13, 38, 63, 160, 180, 185, 189, 190, 219, 248, 253, 275, 297, 318, 355},
new short[]{1, 44, 47, 93, 107, 172, 235, 276, 281, 287, 290, 306, 333, 334, 337, 347, 353, 376},
new short[]{13, 15, 32, 125, 127, 157, 165, 176, 236, 344, 350, 381},
new short[]{47, 65, 93, 134, 159, 174, 218, 282, 318, 336, 358, 373, 379},
new short[]{7, 17, 40, 66, 102, 141, 154, 159, 165, 172, 174, 177, 328, 329, 334, 348, 379, 382},
new short[]{4, 34, 36, 76, 79, 122, 127, 138, 176, 241, 267, 309, 334, 367, 382},
new short[]{9, 17, 33, 46, 90, 103, 125, 138, 144, 157, 185, 198, 224, 250, 260, 291, 326, 343, 349, 377, 381},
new short[]{29, 31, 53, 58, 134, 138, 193, 287, 305, 308, 333, 334},
new short[]{13, 64, 83, 93, 129, 192, 227, 244, 397},
new short[]{7, 8, 14, 78, 85, 103, 138, 175, 176, 200, 203, 234, 301, 313, 361},
new short[]{13, 75, 87, 111, 244, 253, 288, 321, 339, 341, 357, 395},
new short[]{4, 14, 42, 64, 69, 108, 110, 117, 122, 131, 159, 163, 188, 198, 200, 206, 244, 292, 300, 354, 390},
new short[]{14, 37, 73, 87, 129, 135, 144, 176, 182, 300, 346, 352, 380, 383},
new short[]{23, 50, 87, 143, 171, 186, 191, 223, 290, 333, 334, 364, 378, 380, 388, 391, 393},
new short[]{5, 14, 23, 36, 62, 71, 76, 95, 99, 128, 176, 211, 229, 357},
new short[]{12, 33, 47, 70, 81, 90, 97, 119, 122, 131, 189, 190, 191, 235, 244, 253, 320, 350, 359},
new short[]{10, 13, 23, 93, 110, 120, 135, 171, 195, 250, 293, 298, 329, 344, 354},
new short[]{13, 29, 37, 163, 169, 200, 211, 214, 217, 236, 246, 249, 282, 327, 349, 353, 362, 372},
new short[]{5, 13, 23, 41, 57, 62, 76, 89, 111, 135, 195, 234, 248, 314, 334, 341, 349, 380},
new short[]{17, 35, 57, 117, 121, 206, 235, 243, 265, 329, 358, 374},
new short[]{13, 28, 41, 55, 69, 101, 103, 126, 138, 198, 267, 276, 288, 313, 334, 335, 339, 354, 376, 383, 394},
new short[]{11, 13, 19, 36, 38, 58, 75, 124, 232, 235, 265, 286, 298, 330, 333, 359},
new short[]{4, 19, 25, 43, 110, 125, 165, 331, 334, 341, 349, 355, 372},
new short[]{40, 55, 64, 70, 117, 126, 127, 135, 160, 172, 173, 186, 270, 318, 338, 344, 378},
new short[]{122, 176, 198, 238, 246, 284, 286, 290, 318, 329, 337, 381, 394},
new short[]{23, 36, 37, 44, 117, 124, 198, 204, 233, 248, 282, 288, 297, 314, 332, 336, 388},
new short[]{15, 33, 54, 64, 75, 85, 115, 127, 165, 196, 229, 237, 254, 307, 327, 335, 349, 383},
new short[]{22, 87, 121, 127, 161, 180, 248, 250, 276, 313, 324, 347, 349, 355, 357, 359},
new short[]{14, 48, 67, 88, 130, 131, 172, 188, 195, 203, 267, 282, 333, 339, 350, 392},
new short[]{22, 31, 37, 98, 118, 132, 135, 137, 142, 151, 243, 244, 282, 305, 333, 349, 350, 351, 353, 358, 374},
new short[]{15, 42, 67, 75, 125, 134, 189, 255, 261, 309, 334, 350, 380, 382},
new short[]{10, 39, 87, 97, 105, 109, 125, 137, 225, 226, 253, 329, 341, 354, 363, 372},
new short[]{5, 17, 42, 64, 80, 111, 120, 169, 175, 206, 237, 267, 288, 290, 324, 351, 364, 390},
new short[]{3, 33, 55, 75, 91, 97, 103, 132, 187, 220, 232, 234, 240, 288, 301, 330, 336, 337, 338, 340, 359, 374, 380, 382},
new short[]{13, 87, 98, 125, 126, 127, 128, 250, 330, 341, 353, 360, 374, 382, 391},
new short[]{59, 66, 75, 125, 135, 172, 192, 230, 231, 255, 256, 276, 300, 306, 339, 349, 353, 390},
new short[]{25, 36, 56, 90, 107, 125, 127, 142, 165, 195, 244, 246, 319, 347, 355, 375, 380},
new short[]{2, 33, 35, 36, 72, 74, 87, 92, 111, 131, 145, 176, 244, 248, 282, 333, 355, 359},
new short[]{5, 39, 127, 134, 137, 200, 240, 283, 284, 343, 344, 372},
new short[]{9, 32, 37, 80, 96, 104, 110, 117, 154, 176, 244, 297, 298, 339, 353, 374, 381},
new short[]{38, 51, 64, 76, 80, 93, 96, 134, 150, 173, 275, 290, 340, 347, 359, 363, 380},
new short[]{55, 89, 111, 126, 157, 159, 162, 182, 188, 244, 253, 280, 334, 359, 384, 398},
new short[]{59, 64, 75, 81, 97, 105, 115, 125, 155, 198, 248, 262, 319, 323, 376},
new short[]{13, 41, 76, 125, 127, 130, 134, 135, 159, 167, 183, 229, 230, 240, 246, 308, 319, 329, 333, 334, 340, 344, 363, 382},
new short[]{8, 13, 19, 31, 70, 76, 79, 96, 127, 153, 163, 165, 184, 227, 230, 247, 255, 336, 337, 348, 353, 357, 361, 362},
new short[]{71, 87, 111, 121, 130, 142, 150, 160, 175, 224, 248, 314, 336, 353, 357, 359},
new short[]{67, 84, 101, 130, 287, 288, 332, 333, 359, 361, 377},
new short[]{34, 52, 90, 100, 125, 135, 165, 173, 320, 341, 352, 359, 382, 392},
new short[]{13, 18, 39, 55, 62, 87, 248, 255, 290, 327, 349, 353, 355, 360, 383},
new short[]{1, 9, 12, 29, 32, 36, 82, 139, 140, 149, 153, 165, 167, 180, 185, 231, 241, 244, 274, 299, 309, 329, 355, 362},
new short[]{48, 66, 98, 107, 120, 122, 125, 135, 190, 195, 198, 215, 253, 256, 280, 282, 307, 320, 334, 349, 353, 355},
new short[]{1, 7, 13, 25, 64, 98, 139, 144, 166, 176, 206, 236, 262, 330, 362},
new short[]{37, 55, 116, 123, 125, 131, 165, 234, 266, 276, 328, 329, 342, 349, 353, 359, 391},
new short[]{126, 137, 191, 215, 239, 288, 290, 321, 324, 333, 334, 338, 349, 353, 362, 379},
new short[]{50, 57, 87, 93, 98, 115, 134, 148, 174, 229, 251, 260, 285, 298, 313, 348, 349, 350},
new short[]{5, 13, 31, 45, 69, 81, 108, 122, 127, 160, 165, 176, 179, 237, 244, 301, 316, 352, 360},
new short[]{5, 87, 95, 98, 101, 132, 135, 159, 167, 190, 203, 217, 234, 235, 247, 289, 333, 341, 343, 352},
new short[]{22, 56, 66, 85, 87, 93, 126, 127, 163, 230, 243, 248, 254, 280, 301, 305, 334, 357},
new short[]{13, 19, 53, 59, 76, 91, 117, 122, 195, 298, 303, 309, 337, 345, 398},
new short[]{9, 54, 84, 107, 125, 127, 135, 144, 156, 173, 176, 202, 215, 231, 234, 246, 266, 282, 335, 336, 347, 351, 374},
new short[]{11, 15, 30, 31, 40, 57, 58, 87, 88, 113, 186, 244, 245, 256, 308, 334, 377},
new short[]{62, 111, 176, 196, 228, 231, 288, 294, 302, 306, 350, 353, 375, 378, 392},
new short[]{119, 131, 133, 154, 161, 179, 198, 232, 234, 265, 301, 314, 344, 353, 378},
new short[]{67, 84, 123, 172, 175, 176, 182, 229, 290, 359, 360, 375, 383, 393},
new short[]{33, 36, 39, 102, 116, 136, 137, 208, 234, 256, 307, 329, 341, 347, 376, 380},
new short[]{13, 27, 32, 80, 95, 108, 131, 165, 167, 180, 190, 200, 235, 241, 244, 323, 330, 339, 372},
new short[]{1, 18, 37, 62, 67, 82, 85, 118, 125, 147, 159, 169, 174, 243, 284, 307, 313, 318, 355, 391, 396},
new short[]{10, 87, 91, 135, 169, 176, 215, 246, 267, 282, 295, 320, 345, 353, 380},
new short[]{2, 11, 13, 29, 90, 124, 131, 132, 170, 174, 176, 229, 246, 258, 298, 336, 344, 349},
new short[]{14, 37, 42, 71, 128, 152, 185, 218, 288, 304, 315, 353, 362, 380, 391},
new short[]{17, 20, 36, 73, 93, 128, 163, 194, 211, 217, 282, 290, 320, 354, 383},
new short[]{9, 26, 32, 101, 127, 169, 178, 183, 191, 236, 244, 310, 330, 336, 345, 353, 360, 372, 380, 394},
new short[]{7, 13, 64, 78, 81, 90, 115, 133, 164, 169, 244, 246, 269, 278, 290, 292, 310, 320, 353, 360, 364, 366, 380},
new short[]{8, 65, 81, 84, 91, 126, 129, 158, 183, 184, 194, 254, 262, 333, 334, 339, 351, 363, 382},
new short[]{44, 87, 96, 97, 125, 161, 173, 177, 183, 188, 189, 209, 235, 288, 315, 334, 351},
new short[]{50, 56, 60, 62, 67, 71, 105, 149, 154, 158, 164, 167, 185, 221, 285, 288, 308, 337, 344, 353},
new short[]{6, 10, 37, 62, 74, 79, 81, 128, 139, 154, 167, 198, 228, 244, 267, 290, 302, 368, 394},
new short[]{6, 30, 35, 36, 62, 65, 71, 112, 153, 163, 167, 180, 186, 195, 249, 286, 303, 329, 334},
new short[]{158, 241, 282, 324, 332, 334, 351, 353, 363, 365},
new short[]{17, 89, 117, 144, 165, 180, 185, 198, 229, 244, 290, 334, 335, 380},
new short[]{20, 32, 45, 57, 64, 66, 120, 135, 144, 176, 192, 244, 297, 301, 354, 381},
new short[]{1, 7, 35, 62, 74, 122, 159, 170, 172, 238, 239, 307, 308, 338, 349, 350, 359, 366, 368, 375, 382, 383},
new short[]{7, 9, 23, 66, 92, 103, 111, 135, 182, 203, 246, 247, 265, 285, 288, 303, 317, 329, 348},
new short[]{13, 39, 74, 87, 127, 135, 144, 193, 212, 243, 270, 290, 303, 315, 375, 376},
new short[]{33, 36, 40, 59, 101, 120, 127, 244, 285, 287, 309, 339, 391},
new short[]{4, 10, 39, 195, 268, 284, 336, 354, 359, 375, 381},
new short[]{39, 42, 62, 79, 83, 84, 101, 109, 132, 138, 202, 215, 277, 353, 358, 359},
new short[]{10, 39, 46, 73, 84, 87, 132, 170, 192, 219, 232, 246, 288, 320, 337},
new short[]{10, 12, 56, 87, 91, 101, 132, 227, 254, 301, 303, 333, 343, 347, 351},
new short[]{7, 8, 15, 18, 82, 105, 130, 232, 250, 290, 316, 332, 348, 350},
new short[]{36, 109, 110, 125, 154, 191, 193, 246, 265, 348, 349, 350, 378, 383},
new short[]{12, 16, 45, 57, 87, 92, 101, 105, 129, 130, 155, 167, 218, 292, 293, 327, 349, 354, 361},
new short[]{30, 59, 64, 121, 125, 149, 163, 188, 212, 250, 348, 350, 351, 352, 353, 378, 380},
new short[]{1, 69, 130, 138, 194, 200, 239, 260, 264, 357, 380, 381, 382, 396},
new short[]{7, 10, 19, 40, 57, 61, 125, 137, 141, 212, 239, 251, 310, 333, 347, 359, 380, 383},
new short[]{20, 28, 50, 97, 109, 134, 157, 162, 184, 199, 244, 246, 286, 352, 353, 360, 373, 380},
new short[]{35, 62, 87, 96, 122, 127, 136, 142, 148, 155, 165, 186, 196, 227, 354, 380, 388},
new short[]{81, 82, 101, 115, 125, 200, 243, 313, 351, 359, 367},
new short[]{7, 19, 40, 61, 107, 108, 124, 154, 161, 244, 309, 329, 345, 379, 394},
new short[]{10, 27, 48, 66, 75, 103, 116, 122, 128, 221, 228, 319, 322, 350, 377, 398},
new short[]{2, 64, 74, 117, 130, 165, 172, 180, 191, 218, 221, 288, 299, 325, 347, 353, 355, 360},
new short[]{5, 76, 79, 87, 106, 111, 137, 168, 180, 235, 243, 288, 315, 321, 338, 344, 348, 378, 382, 383},
new short[]{0, 29, 31, 37, 40, 50, 88, 100, 129, 134, 137, 144, 174, 186, 203, 254, 310, 313, 329, 341, 359, 364},
new short[]{69, 70, 71, 96, 115, 121, 130, 157, 159, 200, 230, 246, 250, 299, 318, 324, 353, 359, 380, 391},
new short[]{7, 90, 95, 116, 127, 128, 135, 137, 141, 154, 161, 254, 330, 359, 379, 388},
new short[]{10, 14, 56, 91, 108, 125, 130, 167, 211, 228, 246, 258, 280, 306, 324, 333, 336, 338, 379},
new short[]{4, 5, 14, 57, 85, 98, 125, 135, 136, 176, 254, 334, 336, 337, 351, 358, 362, 379, 383},
new short[]{1, 4, 13, 18, 19, 32, 50, 60, 62, 87, 117, 176, 211, 251, 329, 343, 359},
new short[]{38, 56, 94, 103, 117, 125, 129, 144, 159, 176, 244, 251, 253, 324, 345, 353, 386, 390},
new short[]{4, 22, 38, 47, 59, 64, 82, 97, 110, 135, 153, 176, 235, 236, 241, 287, 288, 303, 333, 347, 358, 359, 361},
new short[]{2, 5, 20, 52, 97, 125, 127, 132, 135, 137, 174, 188, 191, 243, 288, 310, 334, 346, 348, 349, 362, 372, 378},
new short[]{19, 35, 55, 98, 125, 131, 134, 147, 153, 246, 255, 390},
new short[]{5, 59, 62, 129, 136, 153, 198, 225, 235, 239, 254, 295, 334, 338, 341, 359, 361},
new short[]{8, 13, 51, 94, 121, 122, 125, 126, 129, 240, 272, 290, 297, 323, 352, 358, 376, 391, 395},
new short[]{6, 111, 116, 122, 125, 131, 135, 164, 175, 200, 212, 221, 267, 287, 319, 328, 334, 344, 378},
new short[]{83, 108, 143, 172, 176, 192, 198, 246, 262, 286, 287, 308, 338, 340, 343, 348, 353, 367, 380, 383},
new short[]{39, 82, 92, 118, 126, 128, 144, 171, 211, 234, 244, 253, 328, 333, 339, 357, 359, 380},
new short[]{37, 62, 64, 81, 97, 122, 125, 127, 137, 211, 246, 344, 360},
new short[]{7, 29, 62, 67, 69, 81, 87, 107, 132, 151, 160, 229, 244, 284, 285, 317, 358, 387, 390},
new short[]{13, 75, 76, 83, 87, 154, 165, 190, 212, 258, 285, 308, 309, 316, 320, 332, 336, 340, 352, 353, 354, 358, 383},
new short[]{9, 19, 29, 46, 122, 125, 127, 130, 170, 171, 174, 180, 182, 232, 282, 290, 359, 362, 367},
new short[]{13, 40, 71, 98, 101, 116, 125, 127, 169, 172, 175, 283, 288, 309, 311, 313, 323, 334, 353, 391},
new short[]{3, 9, 70, 104, 118, 173, 200, 219, 246, 262, 288, 297, 309, 328, 329, 334, 341, 353},
new short[]{32, 89, 93, 131, 132, 142, 199, 200, 214, 246, 287, 298, 307, 339, 348, 349, 357, 358, 368, 372, 391},
new short[]{103, 134, 159, 176, 186, 235, 261, 276, 282, 290, 301, 317, 329, 345, 356},
new short[]{10, 59, 125, 129, 130, 192, 217, 283, 318, 343, 345, 349, 353, 380, 383, 392},
new short[]{19, 76, 79, 102, 107, 126, 155, 161, 180, 253, 288, 289, 290, 314, 329, 333, 334, 360, 368, 378, 394},
new short[]{12, 92, 98, 105, 137, 149, 172, 196, 198, 244, 260, 262, 282, 298, 329, 345, 353, 368, 390},
new short[]{31, 39, 79, 83, 121, 125, 167, 171, 186, 198, 288, 303, 306, 334, 337, 376},
new short[]{13, 20, 36, 57, 98, 108, 114, 165, 171, 225, 226, 262, 269, 305, 309, 351, 377, 389},
new short[]{13, 51, 71, 93, 110, 129, 130, 156, 165, 170, 173, 183, 191, 200, 211, 212, 255, 266, 299, 301, 329, 336, 348},
new short[]{31, 56, 97, 122, 125, 129, 160, 188, 202, 204, 206, 225, 235, 247, 254, 255, 288, 334, 350, 362, 365, 367},
new short[]{9, 32, 37, 70, 75, 87, 88, 96, 125, 130, 162, 163, 168, 169, 257, 285, 308, 310, 337, 373, 392},
new short[]{18, 40, 42, 47, 73, 76, 85, 105, 108, 125, 130, 132, 134, 167, 191, 284, 310, 311, 344, 358, 361, 374, 378, 379},
new short[]{5, 19, 29, 31, 48, 65, 98, 129, 131, 143, 165, 171, 172, 196, 198, 277, 296, 311, 317, 327, 351, 380},
new short[]{51, 69, 96, 98, 117, 123, 130, 131, 148, 161, 168, 172, 176, 184, 202, 324, 332, 336, 348, 392},
new short[]{1, 20, 37, 57, 70, 76, 79, 87, 165, 176, 234, 251, 333, 388},
new short[]{8, 13, 134, 135, 153, 165, 169, 193, 195, 255, 273, 337, 348, 359, 360, 382, 391},
new short[]{2, 14, 53, 71, 83, 127, 136, 144, 149, 208, 234, 235, 293, 301, 347, 352},
new short[]{20, 40, 42, 95, 135, 141, 165, 199, 250, 290, 299, 308, 337, 338, 350, 353, 354, 355, 358, 380},
new short[]{13, 19, 33, 35, 36, 49, 85, 121, 122, 127, 137, 158, 165, 282, 303, 320, 328, 334, 365, 367, 374},
new short[]{17, 37, 123, 126, 127, 139, 140, 143, 167, 185, 192, 235, 254, 275, 315, 340, 349, 353, 362},
new short[]{57, 72, 127, 159, 163, 165, 176, 199, 215, 218, 238, 254, 284, 288, 336, 339, 347, 352, 380, 395},
new short[]{54, 69, 81, 101, 114, 121, 165, 206, 236, 313, 332, 338, 349, 358, 360, 362, 377},
new short[]{29, 37, 43, 120, 127, 176, 193, 244, 246, 254, 284, 288, 336, 339, 372},
new short[]{36, 56, 85, 122, 125, 126, 154, 232, 282, 308, 314, 315, 324, 336, 353, 359, 382},
new short[]{7, 99, 104, 117, 124, 125, 143, 176, 239, 298, 318, 383},
new short[]{13, 20, 71, 90, 108, 122, 176, 186, 214, 231, 247, 262, 267, 280, 286, 300, 332, 358, 377, 380, 385, 390, 393},
new short[]{31, 65, 75, 79, 85, 91, 109, 110, 120, 159, 229, 235, 288, 298, 347, 355, 359, 379, 381},
new short[]{38, 75, 82, 90, 99, 202, 248, 265, 324, 329, 350, 354, 355, 365},
new short[]{7, 15, 72, 90, 117, 125, 140, 144, 171, 198, 269, 271, 282, 305, 325, 338, 343, 353},
new short[]{13, 14, 20, 29, 37, 42, 45, 47, 165, 184, 244, 329, 341, 347, 372},
new short[]{31, 36, 82, 99, 149, 154, 173, 182, 185, 200, 217, 251, 298, 329, 332, 333, 349, 353, 354, 355, 377, 383},
new short[]{32, 44, 45, 52, 93, 97, 108, 114, 120, 144, 155, 172, 236, 240, 267, 272, 282, 288, 329, 333, 334, 343, 381},
new short[]{35, 55, 57, 62, 95, 96, 98, 127, 131, 177, 262, 317, 318, 357, 359, 380, 388},
new short[]{22, 24, 68, 103, 115, 119, 120, 125, 128, 156, 162, 184, 186, 235, 244, 327, 353, 358, 378, 380, 393},
new short[]{29, 37, 62, 67, 81, 83, 93, 104, 110, 129, 132, 142, 172, 274, 298, 354, 380},
new short[]{19, 45, 66, 87, 104, 108, 118, 155, 170, 176, 234, 286, 310, 313, 327, 329, 333, 347, 358, 368, 380, 383, 386},
new short[]{10, 14, 32, 83, 96, 131, 165, 180, 205, 211, 249, 255, 286, 288, 292, 299, 312, 336, 338, 349, 368, 375},
new short[]{2, 13, 48, 75, 85, 98, 116, 125, 126, 128, 135, 136, 151, 188, 195, 243, 280, 289, 333, 339, 349, 378, 382},
new short[]{9, 19, 39, 45, 87, 106, 117, 125, 126, 127, 154, 165, 202, 211, 256, 309, 360, 397, 398},
new short[]{14, 21, 65, 76, 87, 93, 97, 105, 131, 177, 212, 254, 294, 336, 349, 359, 381},
new short[]{36, 55, 65, 70, 87, 93, 96, 98, 108, 127, 254, 337, 352, 359, 375, 380},
new short[]{22, 42, 62, 82, 131, 132, 136, 158, 168, 196, 267, 305, 336},
new short[]{45, 69, 74, 75, 81, 120, 123, 126, 127, 130, 150, 171, 191, 194, 313, 339, 368, 378, 379, 389, 398},
new short[]{35, 43, 85, 98, 122, 131, 135, 176, 189, 250, 259, 277, 288, 303, 333, 336, 345, 376, 381, 387},
new short[]{1, 6, 34, 87, 115, 129, 131, 202, 235, 252, 256, 263, 317, 328, 349, 372, 391},
new short[]{3, 18, 42, 48, 84, 90, 92, 138, 193, 227, 288, 310, 315, 353, 375},
new short[]{2, 10, 31, 66, 124, 145, 240, 314, 334},
new short[]{32, 38, 84, 141, 165, 188, 193, 212, 346, 359, 379, 380},
new short[]{10, 75, 81, 96, 111, 140, 179, 298, 309, 353, 357, 359, 380, 396},
new short[]{2, 34, 121, 127, 132, 134, 184, 234, 244, 251, 262, 290, 308, 359, 380},
new short[]{17, 24, 93, 172, 186, 198, 218, 234, 239, 250, 252, 255, 307, 309, 325, 334, 354, 359},
new short[]{14, 18, 45, 50, 131, 174, 211, 237, 252, 267, 309, 334, 348, 351, 377, 391},
new short[]{32, 61, 87, 97, 125, 126, 132, 184, 249, 252, 273, 284, 288, 339, 383, 398},
new short[]{76, 81, 87, 127, 147, 161, 163, 199, 206, 306, 329, 340, 349, 353, 360, 383},
new short[]{14, 16, 76, 87, 101, 169, 188, 243, 246, 251, 253, 269, 298, 355, 375, 380},
new short[]{32, 79, 87, 103, 117, 125, 127, 177, 244, 301, 305, 317, 333, 338, 340, 342, 391},
new short[]{4, 67, 76, 121, 127, 130, 140, 158, 165, 186, 193, 251, 301, 303, 330, 336},
new short[]{11, 76, 83, 84, 87, 214, 248, 276, 299, 311, 320, 329, 332, 335, 371},
new short[]{2, 4, 19, 40, 42, 71, 98, 119, 121, 137, 167, 262, 288, 295, 306, 339, 350, 382},
new short[]{14, 40, 54, 90, 125, 129, 132, 146, 147, 165, 169, 176, 190, 253, 284, 303, 307, 316, 339, 342, 359, 389},
new short[]{47, 59, 71, 103, 125, 126, 129, 130, 200, 206, 240, 254, 276, 282, 299, 303, 307, 318, 320, 336, 338, 357, 362, 380, 387, 392},
new short[]{4, 22, 58, 102, 113, 115, 153, 167, 188, 212, 262, 286, 305, 333, 348, 354, 360, 371, 379, 386},
new short[]{5, 6, 56, 61, 108, 128, 129, 164, 165, 177, 182, 225, 226, 235, 244, 246, 249, 310, 333, 348, 349, 381, 391},
new short[]{18, 32, 33, 53, 56, 176, 186, 199, 200, 244, 246, 248, 259, 285, 289, 306, 358, 371, 373, 375, 379},
new short[]{40, 43, 70, 76, 83, 84, 90, 93, 101, 125, 159, 204, 276, 282, 304, 320, 339, 351, 353, 367, 391},
new short[]{14, 19, 59, 71, 76, 87, 93, 97, 105, 111, 120, 121, 122, 154, 171, 211, 231, 244, 286, 288, 341, 351},
new short[]{10, 56, 65, 72, 92, 108, 123, 129, 212, 258, 329, 353, 359},
new short[]{5, 76, 124, 127, 161, 172, 188, 244, 250, 266, 290, 318, 347, 351, 369, 382, 391, 395},
new short[]{1, 33, 86, 120, 121, 130, 154, 162, 173, 192, 241, 244, 262, 338, 339, 343, 353, 380, 390},
new short[]{1, 15, 22, 54, 57, 85, 126, 127, 176, 188, 248, 305, 332, 347, 349, 358, 367},
new short[]{91, 111, 122, 125, 130, 178, 190, 224, 225, 226, 235, 286, 308, 329, 334, 345, 346, 349, 358, 362, 367},
new short[]{16, 26, 51, 54, 84, 85, 98, 120, 272, 319, 349, 359, 360, 362, 377, 391, 398},
new short[]{73, 85, 102, 109, 128, 153, 171, 184, 248, 249, 256, 298, 300, 335, 338, 340, 355, 370},
new short[]{9, 108, 122, 131, 164, 168, 173, 176, 195, 218, 235, 286, 341, 350, 353, 358, 375, 377},
new short[]{25, 62, 125, 140, 165, 173, 200, 225, 226, 243, 283, 286, 329, 343, 357, 366, 377},
new short[]{10, 35, 58, 64, 98, 103, 125, 127, 129, 135, 141, 165, 169, 175, 189, 244, 258, 259, 306, 331, 333, 378, 380, 391},
new short[]{54, 87, 89, 99, 116, 125, 129, 221, 246, 269, 324, 335, 348, 351},
new short[]{85, 90, 103, 115, 131, 134, 165, 207, 282, 307, 313, 328, 346, 349, 380, 383, 387, 398},
new short[]{10, 40, 74, 84, 160, 239, 253, 272, 282, 333, 344, 351, 359, 360, 379},
new short[]{32, 38, 54, 74, 76, 117, 163, 171, 176, 217, 227, 250, 251, 280, 329, 330, 350, 378},
new short[]{13, 20, 40, 107, 129, 135, 154, 158, 161, 163, 179, 206, 281, 315, 325, 351, 355, 359, 397},
new short[]{0, 4, 37, 49, 62, 98, 117, 129, 177, 244, 285, 289, 306, 338, 360, 381},
new short[]{36, 38, 43, 61, 71, 87, 120, 128, 172, 200, 235, 247, 251, 282, 299, 329, 341, 352, 355},
new short[]{43, 71, 83, 85, 108, 117, 118, 121, 133, 138, 165, 206, 231, 254, 290, 291, 335, 336, 359, 362, 377},
new short[]{29, 32, 71, 103, 122, 125, 198, 224, 244, 285, 303, 333, 335, 337},
new short[]{54, 55, 82, 87, 101, 108, 127, 229, 230, 269, 290, 306, 349, 353},
new short[]{9, 117, 126, 137, 154, 165, 167, 186, 192, 229, 277, 283, 301, 317, 365, 367, 372, 378},
new short[]{4, 11, 19, 47, 51, 92, 110, 132, 137, 140, 290, 298, 361, 377, 379},
new short[]{23, 83, 98, 134, 165, 170, 186, 190, 253, 269, 308, 322, 327, 332, 335, 344, 398},
new short[]{60, 83, 111, 129, 173, 176, 186, 232, 306, 327, 329, 349, 355},
new short[]{25, 31, 40, 56, 72, 95, 126, 144, 149, 161, 173, 240, 262, 332, 333, 356, 368, 391, 394},
new short[]{91, 127, 134, 144, 155, 158, 161, 232, 251, 280, 287, 353, 380, 394},
new short[]{37, 43, 57, 84, 87, 149, 175, 288, 330, 380},
new short[]{8, 9, 83, 97, 120, 128, 158, 171, 193, 232, 287, 308, 309, 334, 355},
new short[]{39, 40, 62, 82, 94, 98, 101, 144, 147, 205, 290, 333, 339, 353, 372, 397},
new short[]{10, 20, 38, 125, 135, 138, 168, 180, 191, 203, 231, 250, 280, 301, 328, 345, 388},
new short[]{44, 54, 64, 87, 117, 122, 127, 154, 234, 239, 244, 298, 329, 378, 383},
new short[]{13, 62, 70, 97, 121, 176, 244, 267, 282, 318, 324, 334, 341, 353, 386, 388},
new short[]{40, 89, 91, 117, 125, 131, 155, 173, 193, 244, 273, 277, 328, 333, 360, 382},
new short[]{30, 47, 95, 108, 127, 165, 188, 211, 273, 349, 354, 368, 391},
new short[]{19, 52, 87, 98, 100, 122, 125, 157, 159, 215, 217, 235, 254, 309, 336, 344, 349, 382},
new short[]{19, 85, 87, 136, 144, 180, 190, 229, 310, 345, 365, 376, 390},
new short[]{35, 52, 87, 113, 124, 135, 145, 167, 174, 225, 226, 244, 247, 300, 359},
new short[]{10, 35, 69, 103, 129, 144, 165, 180, 230, 232, 329, 335, 353, 359, 371, 390},
new short[]{5, 13, 80, 83, 135, 139, 142, 176, 179, 190, 205, 217, 282, 298, 308, 334, 353, 359},
new short[]{24, 52, 67, 108, 135, 138, 153, 176, 231, 249, 283, 304, 337, 351, 353, 355},
new short[]{90, 93, 127, 132, 136, 163, 165, 196, 284, 306, 353, 383},
new short[]{20, 37, 103, 126, 135, 184, 204, 215, 221, 288, 300, 329, 339, 358, 383},
new short[]{16, 36, 52, 99, 117, 136, 171, 190, 243, 244, 303, 315, 333, 349, 373, 382},
new short[]{0, 57, 69, 98, 125, 129, 132, 158, 165, 190, 191, 193, 198, 254, 256, 285, 288, 303, 339, 346, 351, 391},
new short[]{1, 13, 21, 87, 125, 132, 150, 204, 240, 249, 253, 265, 288, 334, 343, 348, 349, 359},
new short[]{29, 40, 71, 80, 91, 99, 122, 203, 289, 290, 298, 329, 353, 380, 390},
new short[]{2, 5, 36, 57, 93, 102, 135, 140, 314, 343, 398},
new short[]{20, 59, 107, 193, 204, 246, 247, 336, 341, 342, 354, 359, 360, 383},
new short[]{47, 71, 93, 111, 116, 120, 122, 130, 251, 286, 298, 299, 348},
new short[]{21, 52, 56, 69, 76, 118, 120, 125, 137, 274, 280, 324, 327, 335, 339, 340},
new short[]{23, 29, 57, 75, 98, 132, 149, 157, 160, 235, 244, 288, 327, 340, 354, 372, 377},
new short[]{4, 22, 97, 103, 111, 129, 131, 151, 158, 176, 204, 248, 265, 309, 359, 391, 392},
new short[]{15, 17, 73, 105, 115, 170, 186, 228, 255, 317, 321, 339, 349, 379, 380, 381},
new short[]{17, 52, 72, 103, 188, 329, 342, 353, 358, 359, 374, 376, 380, 393},
new short[]{40, 48, 74, 124, 135, 191, 225, 226, 237, 291, 300, 304, 310, 347, 359, 380, 396},
new short[]{2, 36, 47, 57, 122, 125, 174, 188, 203, 224, 255, 325, 353, 359, 387},
new short[]{13, 58, 69, 83, 115, 120, 134, 161, 165, 174, 175, 191, 246, 255, 280, 353, 357, 358, 359, 379},
new short[]{1, 29, 47, 87, 89, 135, 176, 190, 209, 236, 304, 344, 348, 358, 359, 378},
new short[]{8, 13, 40, 52, 58, 61, 71, 125, 144, 168, 189, 210, 260, 337, 338, 340, 347, 376, 380},
new short[]{29, 90, 126, 127, 129, 136, 145, 159, 165, 188, 274, 284, 288, 316, 329, 358, 380},
new short[]{2, 19, 103, 120, 123, 159, 165, 175, 177, 180, 238, 244, 251, 294, 329, 342, 345, 349, 357, 376, 392},
new short[]{41, 42, 59, 71, 81, 98, 101, 117, 159, 171, 180, 240, 285, 290, 299, 344, 353},
new short[]{83, 103, 108, 142, 175, 248, 290, 300, 321, 354, 365, 374, 382},
new short[]{12, 67, 105, 130, 140, 171, 188, 192, 244, 276, 290, 302, 348, 349, 357, 360, 380},
new short[]{4, 13, 36, 65, 75, 160, 165, 185, 198, 235, 293, 324, 327, 333, 345, 347, 375, 383},
new short[]{37, 61, 80, 125, 234, 283, 290, 353, 359, 378, 383},
new short[]{9, 32, 83, 110, 155, 248, 252, 288, 313},
new short[]{37, 48, 52, 93, 167, 170, 179, 244, 267, 288, 296, 333, 335, 355, 374},
new short[]{35, 92, 98, 153, 165, 184, 215, 233, 242, 290, 339, 355},
new short[]{9, 38, 83, 121, 127, 165, 176, 235, 253, 305, 330, 337, 355, 358, 359},
new short[]{35, 117, 122, 125, 132, 136, 183, 235, 254, 280, 285, 286, 329, 334, 338, 353, 372},
new short[]{6, 87, 117, 125, 141, 144, 153, 157, 179, 215, 267, 272, 289, 329, 336, 359},
new short[]{7, 14, 37, 82, 135, 147, 154, 202, 244, 290, 297, 298, 345, 355, 368, 383},
new short[]{105, 135, 173, 244, 255, 280, 288, 299, 304, 307, 337, 338, 341, 344},
new short[]{19, 31, 33, 77, 92, 99, 114, 151, 173, 202, 253, 318, 329, 333, 358, 371},
new short[]{1, 8, 14, 30, 39, 120, 157, 172, 227, 229, 251, 257, 272, 339, 380},
new short[]{19, 98, 171, 191, 213, 246, 289, 353, 357, 366, 374, 383},
new short[]{8, 98, 125, 126, 144, 152, 244, 277, 282, 290, 322, 393},
new short[]{17, 206, 211, 224, 336, 338, 386},
new short[]{52, 55, 71, 99, 105, 191, 211, 215, 224, 246, 290, 300, 336, 339, 361},
new short[]{15, 16, 44, 66, 96, 121, 127, 162, 167, 202, 219, 243, 244, 254, 282, 320, 345, 390},
new short[]{7, 83, 92, 121, 130, 160, 177, 280, 308, 309, 339, 350, 352, 358, 380, 390},
new short[]{67, 122, 144, 148, 170, 173, 184, 222, 280, 374},
new short[]{2, 4, 15, 19, 115, 130, 136, 148, 172, 180, 243, 251, 313, 329, 333, 359, 364},
new short[]{90, 98, 108, 124, 167, 176, 202, 254, 286, 351, 359},
new short[]{80, 126, 135, 167, 212, 242, 243, 256, 283, 286, 295, 327, 337, 340, 346, 357, 358, 364},
new short[]{19, 108, 125, 132, 149, 172, 180, 186, 200, 254, 286, 296, 339, 344, 350, 359, 391},
new short[]{62, 65, 67, 105, 127, 129, 132, 250, 298, 307, 334, 344, 359, 383},
new short[]{31, 59, 87, 107, 121, 131, 132, 160, 244, 246, 247, 253, 344, 360, 394},
new short[]{4, 39, 76, 125, 130, 148, 168, 170, 191, 196, 298, 306, 327, 338, 345, 349, 360, 375},
new short[]{13, 14, 32, 84, 98, 122, 126, 156, 188, 235, 255, 330, 336, 338, 375, 380, 389},
new short[]{5, 18, 31, 54, 71, 74, 76, 81, 87, 93, 126, 129, 182, 303, 327, 353, 359, 373, 391},
new short[]{13, 37, 64, 137, 138, 180, 244, 247, 251, 253, 269, 284, 308, 344, 374, 376},
new short[]{5, 7, 10, 23, 35, 125, 168, 169, 187, 191, 192, 313, 337, 340, 342, 365},
new short[]{62, 67, 122, 125, 165, 190, 217, 243, 254, 256, 265, 299, 318, 353, 394},
new short[]{4, 24, 62, 92, 109, 118, 134, 143, 144, 176, 190, 199, 221, 299, 349, 380},
new short[]{22, 35, 64, 74, 92, 113, 161, 172, 193, 282, 287, 307, 359, 393},
new short[]{37, 50, 66, 75, 76, 78, 82, 87, 139, 159, 172, 176, 188, 231, 352, 371},
new short[]{19, 31, 75, 121, 144, 152, 163, 171, 172, 198, 243, 246, 285, 288, 289, 333, 344, 347, 357, 398},
new short[]{1, 15, 17, 51, 57, 65, 69, 127, 241, 244, 254, 259, 329, 336, 358},
new short[]{9, 95, 117, 121, 125, 137, 204, 242, 247, 301, 309, 314, 334, 339, 350, 354, 358},
new short[]{9, 61, 96, 111, 130, 163, 180, 211, 225, 226, 241, 253, 282, 283, 346, 355, 359, 380, 383},
new short[]{94, 117, 121, 124, 126, 130, 135, 172, 199, 232, 286, 325, 336, 352, 362, 375},
new short[]{110, 125, 163, 250, 265, 303, 329, 334, 391},
new short[]{47, 72, 76, 111, 125, 157, 169, 245, 254, 285, 287, 297, 298, 336, 353, 359, 383},
new short[]{62, 93, 115, 125, 127, 130, 174, 231, 308, 310, 329, 333, 355, 359, 390},
new short[]{44, 116, 163, 167, 180, 191, 200, 245, 254, 329, 343, 345, 354, 364},
new short[]{31, 62, 105, 108, 144, 145, 162, 173, 177, 191, 198, 247, 249, 344, 345, 348, 353},
new short[]{29, 65, 66, 74, 83, 87, 125, 148, 165, 228, 334, 353, 359, 380, 383, 391},
new short[]{2, 15, 125, 130, 239, 290, 312, 336, 337, 341, 398},
new short[]{40, 76, 87, 114, 119, 120, 165, 229, 265, 313, 324, 349, 358, 383},
new short[]{48, 62, 87, 91, 103, 186, 195, 212, 214, 315, 322, 327, 330, 338, 339},
new short[]{9, 32, 85, 108, 135, 191, 224, 237, 257, 288, 307, 310, 313, 318, 329, 337, 352, 395},
new short[]{87, 93, 102, 112, 129, 154, 171, 236, 317, 320, 349, 350, 359, 380},
new short[]{1, 14, 92, 111, 137, 140, 186, 290, 329, 336, 354, 355, 378, 379, 383},
new short[]{7, 26, 37, 47, 84, 101, 144, 153, 175, 180, 198, 232, 243, 305, 333, 353, 357, 383},
new short[]{20, 58, 76, 93, 99, 127, 134, 154, 188, 206, 246, 312, 313, 324},
new short[]{2, 12, 117, 125, 160, 167, 188, 206, 279, 285, 287, 301, 329, 332, 333, 336, 344, 362},
new short[]{2, 76, 126, 127, 137, 165, 244, 288, 290, 339, 346, 351, 359, 365, 383},
new short[]{66, 108, 136, 151, 174, 265, 344, 351, 353, 357, 378, 386},
new short[]{8, 76, 87, 90, 111, 116, 124, 176, 198, 334, 337, 349, 359, 379, 394},
new short[]{32, 36, 42, 76, 81, 125, 127, 205, 227, 262, 280, 288, 326, 336, 390, 398},
new short[]{9, 32, 65, 83, 89, 93, 97, 122, 129, 178, 180, 215, 241, 246, 323, 332, 353, 362, 364, 380},
new short[]{5, 24, 56, 127, 130, 155, 184, 191, 217, 235, 245, 339, 344, 358, 359, 362, 380},
new short[]{14, 40, 64, 71, 93, 108, 131, 165, 188, 204, 217, 235, 237, 241, 248, 308, 309, 318, 380, 387},
new short[]{17, 29, 34, 74, 125, 175, 184, 196, 211, 275, 301, 318, 327, 334, 349, 355, 358, 368},
new short[]{15, 45, 110, 111, 116, 129, 132, 211, 247, 275, 286, 317, 333, 334, 377, 383},
new short[]{4, 5, 59, 87, 103, 124, 125, 127, 130, 165, 241, 265, 299, 353, 360},
new short[]{31, 120, 124, 135, 154, 197, 235, 243, 247, 248, 258, 309, 320, 335, 357},
new short[]{50, 125, 127, 130, 137, 147, 171, 172, 267, 289, 301, 308, 325, 334, 337, 353, 360, 374, 391},
new short[]{62, 64, 69, 87, 111, 118, 129, 134, 212, 239, 244, 246, 250, 254, 307, 322, 329, 370, 372},
new short[]{54, 92, 128, 160, 198, 244, 248, 255, 284, 314, 335, 349, 358, 360, 376, 380},
new short[]{9, 13, 29, 54, 72, 89, 110, 122, 126, 139, 158, 159, 163, 230, 304, 306, 313},
new short[]{1, 9, 54, 95, 108, 132, 176, 193, 243, 251, 339, 378},
new short[]{0, 96, 99, 135, 137, 184, 212, 232, 251, 315, 334},
new short[]{140, 157, 165, 182, 235, 294, 314, 349, 354, 365},
new short[]{14, 18, 31, 56, 117, 125, 138, 227, 246, 283, 334, 345, 352, 357, 361},
new short[]{0, 71, 82, 130, 131, 144, 161, 235, 247, 301, 333, 335, 345, 353, 355, 359, 360, 374},
new short[]{6, 23, 35, 117, 125, 141, 169, 200, 244, 288, 298, 338, 353, 379},
new short[]{10, 98, 125, 127, 138, 153, 219, 244, 307, 350, 353, 366, 367},
new short[]{9, 32, 40, 122, 126, 127, 170, 176, 300, 334, 350, 391},
new short[]{6, 13, 31, 87, 89, 97, 125, 165, 171, 173, 176, 244, 331, 348, 373},
new short[]{10, 61, 87, 105, 123, 125, 127, 195, 260, 265, 323, 361, 362},
new short[]{2, 20, 90, 124, 353, 354, 378, 382},
new short[]{5, 48, 58, 83, 98, 117, 125, 126, 196, 198},
new short[]{13, 37, 50, 64, 66, 79, 99, 132, 135, 244, 247, 380},
new short[]{57, 165, 235, 238, 248, 272, 287, 299, 327, 329, 334, 350, 353, 380},
new short[]{55, 66, 118, 125, 130, 169, 250, 255, 271, 314, 324, 338, 353},
new short[]{7, 31, 62, 84, 103, 105, 111, 126, 132, 149, 154, 191, 250, 334, 372, 375},
new short[]{56, 81, 114, 117, 120, 124, 127, 128, 154, 254, 290, 317, 345, 354},
new short[]{4, 13, 86, 101, 153, 191, 193, 231, 243, 258, 283, 288, 308, 353, 387, 392},
new short[]{5, 37, 58, 62, 67, 84, 87, 176, 237, 267, 333, 334, 347},
new short[]{1, 7, 74, 110, 165, 168, 182, 233, 288, 305, 309, 315, 347, 351, 353, 358, 360, 375},
new short[]{57, 84, 129, 138, 165, 243, 244, 259, 280, 282, 290, 380, 383}
};
    }
}