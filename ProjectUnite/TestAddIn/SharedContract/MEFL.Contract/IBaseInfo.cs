using System;
using MEFL.Arguments;

namespace MEFL.Contract;

public interface IBaseInfo
{
	object Title { get; }

	object Icon { get; }

	Uri PulisherUri { get; }

	Uri ExtensionUri { get; }

	void SettingsChange(SettingArgs args);
}
