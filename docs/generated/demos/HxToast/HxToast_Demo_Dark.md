# HxToast_Demo_Dark.razor

```razor
<HxToastContainer>
	<HxToast HeaderText="Bootstrap" HeaderIcon="BootstrapIcon.InfoCircleFill" ContentText="This toast is in dark mode." data-bs-theme="dark" />
	<HxToast HeaderText="Warning" Color="ThemeColor.Warning" ContentText="Dark mode combined with a color variant." data-bs-theme="dark" />
</HxToastContainer>

<HxToastContainer data-bs-theme="dark">
	<HxToast HeaderText="Container" ContentText="All toasts in this container are in dark mode." />
	<HxToast Translucent="true" HeaderText="Translucent" ContentText="Dark translucent toast." />
</HxToastContainer>

```
