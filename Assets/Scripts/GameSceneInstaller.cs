using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameSceneInstaller : MonoInstaller<GameSceneInstaller>
{
	public override void InstallBindings()
	{
		Container.Bind<HUD>().FromComponentInHierarchy().AsSingle();
	}
}