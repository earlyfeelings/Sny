<?php
declare(strict_types=1);

namespace App\Factory\Forms;


use Nette\Forms\Form;
use Nette\SmartObject;

class LoginFormFactory
{

	use SmartObject;

	private FormFactory $formFactory;



	function __construct(
		FormFactory $formFactory
	) {
		$this->formFactory = $formFactory;
	}




	function create(): Form {
		$form = $this->formFactory->create();

		$form->addText('username', 'Uživatelské jméno')
			->setHtmlAttribute('class', 'form-control')
			->setRequired('Zadejte prosím %label.');

		$form->addPassword('password', 'Heslo')
			->setHtmlAttribute('class', 'form-control')
			->setRequired('Zadejte prosím %label.');

		$form->onSuccess[] = [$this, 'formSuccess'];
		$form->onValidate[] = [$this, 'formValidate'];
		return $form;
	}




	/** @internal */
	function formSuccess(Form $form, array $values): Form {
		$this->doPersist();

		return $form;
	}


	/** @internal */
	function formValidate(Form $form): Form {


		return $form;
	}



	private function doPersist()  {

	}





}
