<?php

declare(strict_types=1);

namespace App\Presenters;


use App\Factory\Forms\LoginFormFactory;
use Nette\Application\UI\Form;

final class HomepagePresenter extends BasePresenter
{

	private LoginFormFactory $loginFormFactory;

	function __construct(
		LoginFormFactory $loginFormFactory
	) {
		$this->loginFormFactory = $loginFormFactory;
		parent::__construct();
	}

	function renderDefault(): void
	{


	}


	function createComponentLoginForm(): Form	{
		return $this->loginFormFactory->create();
	}

}
