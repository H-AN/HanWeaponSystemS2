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
    <li><strong>https://www.youtube.com/watch?v=CeNDiYo2XsI&t=27s</strong></li>
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
<p>This plugin uses hidden commands for weapon spawning, so please do not reveal your password commands.</p>
<p></p>
</div>
