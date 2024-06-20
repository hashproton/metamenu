<script lang="ts">
	import ListFilter from 'lucide-svelte/icons/list-filter';

	import { Badge } from '$lib/components/ui/badge/index.js';
	import { Button } from '$lib/components/ui/button/index.js';
	import * as Card from '$lib/components/ui/card/index.js';
	import * as DropdownMenu from '$lib/components/ui/dropdown-menu/index.js';
	import * as Table from '$lib/components/ui/table/index.js';

	let { data } = $props();
</script>

<main
	class="grid flex-1 items-start gap-4 p-4 sm:px-6 sm:py-0 md:gap-8 lg:grid-cols-3 xl:grid-cols-3"
>
	<div class="grid auto-rows-max items-start gap-4 md:gap-8 lg:col-span-2">
		<div class="flex justify-end gap-2">
			<DropdownMenu.Root>
				<DropdownMenu.Trigger asChild let:builder>
					<Button variant="outline" size="sm" class="h-7 gap-1 text-sm" builders={[builder]}>
						<ListFilter class="h-3.5 w-3.5" />
						<span class="sr-only sm:not-sr-only">Filter</span>
					</Button>
				</DropdownMenu.Trigger>
				<DropdownMenu.Content align="end">
					<DropdownMenu.Label>Filter by</DropdownMenu.Label>
					<DropdownMenu.Separator />
					<DropdownMenu.CheckboxItem checked>All</DropdownMenu.CheckboxItem>
					<DropdownMenu.CheckboxItem>Active</DropdownMenu.CheckboxItem>
					<DropdownMenu.CheckboxItem>Inactive</DropdownMenu.CheckboxItem>
				</DropdownMenu.Content>
			</DropdownMenu.Root>
			<a href="/tenants/create">
				<Button variant="default" size="sm">Add Tenant</Button>
			</a>
		</div>
		<Card.Root>
			<Card.Header class="px-7">
				<Card.Title>Tenants</Card.Title>
				<Card.Description>All businesses of metamenu.</Card.Description>
			</Card.Header>
			<Card.Content>
				{#if data.response == null || data.response.items.length === 0}
					<div class="flex items-center justify-center h-32">
						<p class="text-muted
							-foreground">No tenants found.</p>
					</div>
				{:else}
				<Table.Root>
					<Table.Header>
						<Table.Row>
							<Table.Head>Name</Table.Head>
							<Table.Head class="hidden sm:table-cell">Status</Table.Head>
							<Table.Head class="text-right">Products</Table.Head>
						</Table.Row>
					</Table.Header>
					<Table.Body>
						{#each data.response.items as tenant}
							<Table.Row>
								<Table.Cell>
									<div class="font-medium">
										<a href={`/tenants/${tenant.id}`}>{tenant.name}</a>
									</div>
									<div class="hidden text-sm text-muted-foreground md:inline">
										<a href={'notimplemented'} target="_blank">{'notimplemented'}</a>
									</div>
								</Table.Cell>
								<Table.Cell class="hidden sm:table-cell">
									<Badge class="text-xs" variant={tenant.isActive ? 'destructive' : 'default'}>
										{tenant.isActive ? 'Active' : 'Inactive'}
									</Badge>
								</Table.Cell>
								<Table.Cell class="text-right">32</Table.Cell>
							</Table.Row>
						{/each}
					</Table.Body>
				</Table.Root>
				{/if}
			</Card.Content>
		</Card.Root>
	</div>
	<div>
		<Card.Root class="overflow-hidden">
			<Card.Header class="flex flex-row items-start bg-muted/50">
				<div class="grid gap-0.5">
					<Card.Title class="group flex items-center gap-2 text-lg">
						Status
						<Button
							size="icon"
							variant="outline"
							class="h-6 w-6 opacity-0 transition-opacity group-hover:opacity-100"
						></Button>
					</Card.Title>
					<Card.Description>Date: November 23, 2023</Card.Description>
				</div>
			</Card.Header>
			<Card.Content class="p-6 text-sm">
				<div class="grid gap-3">
					<ul class="grid gap-3">
						<li class="flex items-center justify-between">
							<span class="text-muted-foreground">Active</span>
							<span>2</span>
						</li>
						<li class="flex items-center justify-between">
							<span class="text-muted-foreground">Inactive</span>
							<span>5</span>
						</li>
					</ul>
				</div>
			</Card.Content>
		</Card.Root>
	</div>
</main>
