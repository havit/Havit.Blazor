# HxButtonGroup_Demo_Radios.razor

```razor
<EditForm Model="@model">
    <HxButtonGroup>
        <InputRadioGroup @bind-Value="model.selectedRadio">
            <label class="btn-check btn-outline theme-primary">
                <InputRadio Value="@("Radio 1")" />
                Radio 1
            </label>

            <label class="btn-check btn-outline theme-primary">
                <InputRadio Value="@("Radio 2")" />
                Radio 2
            </label>

            <label class="btn-check btn-outline theme-primary">
                <InputRadio Value="@("Radio 3")" />
                Radio 3
            </label>
        </InputRadioGroup>
    </HxButtonGroup>
</EditForm>

@code {
    private DemoModel model = new();

    class DemoModel
    {
        public string selectedRadio = "Radio 1";
    }
}

```
