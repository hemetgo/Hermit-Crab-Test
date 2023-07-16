using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameSceneInstaller : MonoInstaller<GameSceneInstaller>
{
	public override void InstallBindings()
	{
		Container.Bind<HUD>().FromComponentInHierarchy().AsSingle();
		Container.Bind<Player>().FromComponentInHierarchy().AsSingle();
		Container.Bind<PlayerHealthBar>().FromComponentInHierarchy().AsSingle();
		Container.Bind<PowerUpManager>().FromComponentInHierarchy().AsSingle();
		Container.Bind<WaveManager>().FromComponentInHierarchy().AsSingle();
		Container.Bind<EndScreen>().FromComponentInHierarchy().AsSingle();
		Container.Bind<PauseManager>().AsSingle();
	}
}