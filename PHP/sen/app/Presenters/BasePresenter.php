<?php

declare(strict_types=1);

namespace App\Presenters;

use Nette\Application\UI\Presenter;
use Nette\Utils\Random;


abstract class BasePresenter extends Presenter
{


	function beforeRender() {
		$this->template->version = Random::generate(5);
		$this->template->projectName = "Sny.cz";
	}

}
