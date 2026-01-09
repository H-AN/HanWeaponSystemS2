<div align="center"><h1><img width="600" height="131" alt="68747470733a2f2f70616e2e73616d7979632e6465762f732f56596d4d5845" src="https://github.com/user-attachments/assets/d0316faa-c2d0-478f-a642-1e3c3651f1d4" /></h1></div>

<div class="section">
<div align="center"><h1>HanWeaponSystemS2 for Sw2 / 自定义武器系统 for Sw2 </h1></div>
<div align="center"><strong>描述:</strong> 自定义武器系统，给予玩家自定义武器,支持自定义模型皮肤,各种属性等。</p></div>
<div align="center"><strong>The custom weapon system allows players to customize their weapons, supporting custom models, skins, and various attributes.</p></div>
</div>

<div align="center">

[![ko-fi](https://ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/Z8Z31PY52N)
  

</div>

<div class="section">
<h2>视频预览/video</h2>
<ul>
    <li><strong>https://www.bilibili.com/video/BV1dPqKBeEXf</strong></li>
    <li><strong>https://www.youtube.com/watch?v=CeNDiYo2XsI&t</strong></li>
</ul>
</div>

<div class="section">
<h2>功能特点/Features</h2>
<ul>
    <li>支持 <strong>支持自定义模型皮肤</strong></li>
    <li>支持 <strong>自定义武器属性(需要自己填写vdata文件并上传至创意工坊)</strong></li>
    <li>支持 <strong>命令控制武器,可自由填写服务器隐藏指令,用于其他插件(如空投支援插件)</strong></li>
    <li>Supports custom model skins</strong></li>
    <li>Supports custom weapon attributes (requires manually filling in the vdata file and uploading it to the Steam Workshop)</strong></li>
    <li>Supports command control weapons, allowing free input of hidden server commands for use with other plugins (such as airdrop support plugins)</strong></li>
</ul>
</div>

<div class="section">
<h2>示例创意工坊文件(含模型与vdata)</h2>
<h2>Example Workshop files (including models and vdata)</h2>
<pre><code>
	
插件可结合以下创意工坊资源使用（示例）/The plugin can be used in conjunction with the following Workshop resources (examples)：

3450081072

要使用创意工坊资源,需要服务器安装metamod插件 multiaddonmanager 来管理服务器和玩家使用下载和安装创意工坊资源

安装multiaddonmanager插件后 在game\csgo\cfg\multiaddonmanager\multiaddonmanager.cfg配置文件中
 
找到第一行 mm_extra_addons  "3450081072"

把资源ID填写上去 等待服务器下载资源完毕 玩家进服会自动下载资源

之后用 Source2Viewer 软件 打开资源包 查看资源内的 模型路径与vdata

之后根据需要填写到HanWeaponSystemConfig.jsonc内使用

To use Workshop resources, the server needs to install the Metamod plugin multiaddonmanager to manage downloading and installing Workshop content for both the server and players.

After installing the multiaddonmanager plugin, go to the configuration file:

game\csgo\cfg\multiaddonmanager\multiaddonmanager.cfg

Find the first line:

mm_extra_addons "3450081072"

Enter the Workshop resource ID here.
After that, wait for the server to finish downloading the resource. When players join the server, the content will be downloaded automatically.

Then use the Source2Viewer tool to open the resource package and check the model paths and VData inside.

Finally, fill the required information into HanWeaponSystemConfig.jsonc according to your needs.

</code></pre>
</div>
<div class="section">
	
<div class="section">
<h2>制作自定义武器需要使用Vdata文件</h2>
<h2>Creating custom weapons requires using Vdata files.</h2>
<pre><code>
  
    在vdata中最后 增加一个新的武器数据组, 并填写新的vdataname 例如 weapon_doomak47 作为新的一组(数据可以由原始武器作为模板)
    Finally, add a new weapon data group in vdata and fill in a new vdataname, such as weapon_doomak47, as the new group (the data can be based on the original weapon as a template).
    weapon_doomak47 = 
    {
        (原始数据太长只节选需要修改的部分作为示例/具体参照vdata示例文件内容进行学习修改)
        (The original data is too long; only the part that needs to be modified is selected as an example.)
        
        (Refer to the vdata example file for detailed study and modification.)
        
        (修改 m_szModel_AG2 用于新武器的模型路径/Modify m_szModel_AG2 to use the model path for the new weapon.)
        
        m_szModel_AG2 = resource_name:"phase2/weapons/aquaz/_hoshics/slot1/ak47doom/ak47doom_ag2.vmdl" 
    }
    新的武器vdata文件需要编译成 vdata_c 之后上传至创意工坊
</code></pre>
</div>
<div class="section">

<div class="section">
<h2>配置文件示例/Configuration Example</h2>
<pre><code>{
  "HanWeaponSystemCFG": {
    "WeaponsList": [
      {
    		"CustomName": "Doom",     //填写自定义名称(用于ui显示 武器属性定义)
    		"Command": "sw_IAzQYQgCEHOEizJK",     //创建指令,自定义服务器指令(请勿泄露)
    		"ClassName": "weapon_mp5sd",     //武器原始模板(用于创建模板武器,配合自定义模型)
    		"VdataName": "weapon_doomak47",     //vdata名称 根据你自己定义的武器数据组 来填写 
    		"Definition": 23,     //武器数据(可以使用原始武器数据 例如 weapon_mp5sd 是23 也可以 用其他武器的数据 制作出更多搭配)
    		"Damage": "+75",     //伤害加成 支持  + - * / 运算符,自由定义武器伤害 不填写符号默认 +
    		"knock": "30",     //击退力
    		"MaxClip": 35,     //弹匣
    		"ReserveAmmo": 1000,     //备用弹药
    		"Rate": 0.1,     //攻速, 越小越快(攻击间隔)
    		"NoRecoil": true,     //是否开启无后坐力
    		"KillIcon": "weapon_ak47",     //击杀显示图标
    		"Slot": 0,     //武器槽位 , 0主武器 1手枪 2刀 3手雷 
    		"PrecacheModel": "phase2/weapons/aquaz/_hoshics/slot1/ak47doom/ak47doom_ag2.vmdl",     //预缓存自定义模型 要与 m_szModel_AG2 一致
    		"PrecacheSoundEvent": "soundevents/soundevents_customweapon.vsndevts"     //预缓存声音事件 (由于cs2开枪音效由模型定义,声音事件驱动,需要模型搭配使用)
		},
	    {
	        "CustomName": "AS50狙击步枪",    //Custom display name for UI and weapon attribute definitions.
	        "Command": "sw_sb2YWrszaUeZkbfx",    //Server-side custom command used to create this weapon (keep it secret).
	        "ClassName": "weapon_awp",    //Base weapon class template. Used to spawn the original weapon before applying custom model.
	        "VdataName": "weapon_awp_plus8",     //VData identifier. Fill this according to the custom weapon configuration you defined.
	        "Definition": 9,    //Weapon definition index. Can use the original index (e.g., MP5SD = 23) or reuse another weapon’s data to create mixed behaviors.
	        "Damage": "+100",    //Damage modifier. Supports operators + - * /. Without an operator, “+” is used by default.
			"knock": "500",    //Knockback force applied on hit.
	        "MaxClip": 10,    //Magazine size.
	        "ReserveAmmo": 1000,    //Ammo reserve amount.
	        "Rate": 1.0,    //Attack interval. The smaller the value, the faster the fire rate.
	        "NoRecoil": false,    //Whether to enable zero recoil.
	        "KillIcon": "weapon_awp",    //Kill-feed icon to display when this weapon kills a player.
	        "Slot": 0,    //Weapon slot. 0 = primary, 1 = pistol, 2 = knife, 3 = grenades.
	        "PrecacheModel": "phase2/weapons/7ychu5/_hoshics/slot1/as50_dummy/as50.vmdl",    //Custom weapon model that must be precached. Must match the m_szModel_AG2 model used by the weapon.
	        "PrecacheSoundEvent": "soundevents/as50.vsndevts"    //Precache sound event file. CS2 firing sounds are controlled by models and sound events, so the custom model must include its sound definitions.
	      }
	]
	}
}</code></pre>
<p>武器元数据对照表查看/Weapon metadata lookup table</p>
<p>https://github.com/H-AN/CS2WeaponCode/blob/main/Code</p>
</div>
<div class="section">

<div class="section">
<h2>注意事项/Precautions</h2>
<p>由于武器实体数据无法被热更新,所以插件配置热重载无效!!</p>
<p>Because weapon entity data cannot be hot-updated, hot reloading of plugin configurations is ineffective!</p>
<p>武器使用自定义隐藏指令进行发放,请妥善保管自己的隐藏指令密码防止泄露!!</p>
<p>This plugin uses hidden commands for weapon spawning, so please do not reveal your password commands.</p>
<p></p>
</div>

<div class="section">
<h2>武器发放例子/Examples of weapons distribution</h2>
<p>你可以通过这个菜单例子,对武器进行发放操作</p>
<p>You can use this menu example to dispense weapons.</p>

https://github.com/H-AN/SimpleGiveFreeCustomWeaponMenu-example-

<p></p>
</div>

<div class="section">
<h2>关于自定义武器音效与动画的说明/Instructions on Custom Weapon Sound Effects and Animations</h2>
<p>1.由于CS2更新了AG2所以自定义动画暂时无法使用/Custom animations are temporarily unavailable due to the AG2 update in CS2.</p>
</div>

<div class="section">
<h2>由于valve暂未公开AG2框架的编译器,所以模型暂时无法添加声音事件,或许未来valve公开编译器后此方法才可以使用</h2>
<h2>Since Valve has not yet released the compiler for the AG2 framework, sound events cannot be added to the model for the time being. This method may be available once Valve releases the compiler in the future.</h2>
<p>2.自定义音效原理/Custom sound effects principle</p>
<p>①通过在vdata中将CS2原版武器的所有音量改为 0 以屏蔽所有原始武器开枪音效/This disables all original weapon firing sounds by changing the volume of all original CS2 weapons to 0 in vdata.</p>
<p><img width="581" height="634" alt="sound1" src="https://github.com/user-attachments/assets/366ed204-ff16-4406-8604-f1ea70e27d7c" /></p>
<p>②模型作者在武器动画中增加AE_CL_PLAYSOUND事件,事件为soundevent文件内定义的事件音效/The model creator added the AE_CL_PLAYSOUND event to the weapon animation; this event is a sound effect defined in the soundevent file.</p>
<p><img width="510" height="393" alt="sound2" src="https://github.com/user-attachments/assets/8cb461da-099b-47ec-8898-1c2c05c6dd3c" /></p>
<p>当模型中动画定义了音效,玩家使用武器的时候动画播放驱动声音事件播放音效(所以我们需要在插件中预缓存声音事件,声音事件中定义的就是这把武器的各项音效)</p>
<p>When the model's animations define sound effects, the animation plays when the player uses the weapon, driving the sound events to play the sound effects (so we need to pre-cachate sound events in the plugin, and the sound events define the various sound effects for this weapon).</p>
</div>



