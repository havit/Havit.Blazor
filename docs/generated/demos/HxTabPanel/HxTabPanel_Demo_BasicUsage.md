# HxTabPanel_Demo_BasicUsage.razor

```razor
<HxTabPanel InitialActiveTabId="tab2">
	<HxTab Title="First tab">
		<Content>This is the first tab.</Content>
	</HxTab>
	<HxTab Id="tab2">
		<TitleTemplate>Second tab</TitleTemplate>
		<Content>This is the second tab. This tab is initially active.</Content>
	</HxTab>
	<HxTab Enabled="false">
		<TitleTemplate>Disabled tab</TitleTemplate>
		<Content>This is the disabled tab.</Content>
	</HxTab>
</HxTabPanel>

```
